using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using NPTestbench.Models;

public class DataService : BackgroundService, IDisposable
{

    private readonly ConfigurationService _configurationService;
    private readonly DataNotifier _dataNotifier;
    private readonly CommunicationService _communicationService;

    private const int SAMPLE_DELAY_LOW = 1000 / 1;
    private const int SAMPLE_DELAY_HIGH = 1000 / 3;
    private int? _runId;

    Device[] devices = [];
    private DataNotifier.DataState _dataState;

    public DataService(ConfigurationService configurationService, DataNotifier dataNotifier, CommunicationService communicationService)
    {
        _configurationService = configurationService;
        _dataNotifier = dataNotifier;
        _communicationService = communicationService;
        _dataState = new DataNotifier.DataState();

        _configurationService.ActiveConfigurationChanged += async () =>
        {
            devices = (await _configurationService.GetActiveConfiguration()).Devices.ToArray();
        };
    }

    public async Task WriteDevice(Device device, ushort value) {
        var writeChannel = device.DeviceChannels
            .Where(dc => !dc.IsRead)
            .First()
            .Channel;
        await _communicationService.WriteChannel(writeChannel, value);
        await ReadAll();
    }

    public async Task ReadAll()
    {   
        Dictionary<int,float> values;
        try {
            values = await _communicationService.ReadDevices(devices);
        } catch(InvalidOperationException) {
            Console.WriteLine("Network error: Could not reach device");
            _communicationService.Reconnect();
            return;
        } catch(SocketException) {
            Console.WriteLine("Network error: Socket closed");
            _communicationService.Reconnect();
            return;
        }

        // Compute device values
        foreach (var device in devices) 
        {
            if (!_dataState.DeviceStates.ContainsKey(device.Id))
            {
                _dataState.DeviceStates.Add(device.Id, new DataNotifier.DeviceState()
                {
                    Id = device.Id,
                    Name = device.Name,
                    DrawingId = device.DrawingID,
                });
            }

            var channelValues = device.DeviceChannels
                .Where(dc => dc.IsRead) // Select only read channels
                .OrderBy(dc => dc.Order) // Sort by the order
                .Select(dc => dc.Channel) // Jump to the channel
                .Select(c => values[c.Id]) // Use channel id to get value from read results
                .ToArray();

            var value = CalibrationFunctions.Calibrate(device.CalibrationFunctionName, channelValues);
            _dataState.DeviceStates[device.Id].Value = value;
            _dataState.DeviceStates[device.Id].ValueRunMaximum = Math.Max(_dataState.DeviceStates[device.Id].ValueRunMaximum, value);
            _dataState.DeviceStates[device.Id].ValueRunMinimum = Math.Min(_dataState.DeviceStates[device.Id].ValueRunMinimum, value);
        }

        // Save to log if we are running
        if (_runId != null)
        {
            DateTime timestamp = DateTime.Now;
            using (var context = new DataContext())
            {
                foreach (KeyValuePair<int, float> kv in values) {
                    var measurement = new Measurement()
                    {
                        ChannelId = kv.Key,
                        RunId = (int)_runId,
                        Timestamp = timestamp,
                        Value = kv.Value
                    };
                    context.Measurements.Add(measurement);
                }

                foreach (Device device in devices) {
                    var channelValues = device.DeviceChannels
                        .Where(dc => dc.IsRead) // Select only read channels
                        .OrderBy(dc => dc.Order) // Sort by the order
                        .Select(dc => dc.Channel) // Jump to the channel
                        .Select(c => values[c.Id]) // Use channel id to get value from read results
                        .ToArray();
                    
                    var calibratedValue = new CalibratedValue()
                    {
                        DeviceId = device.Id,
                        RunId = (int)_runId,
                        Timestamp = timestamp,
                        Value = _dataState.DeviceStates[device.Id].Value
                    };
                    context.CalibratedValues.Add(calibratedValue);
                }
                context.SaveChanges();
            }
        }

        // TODO: Should not be published on every read
        await PublishDataState();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        devices = (await _configurationService.GetActiveConfiguration()).Devices.ToArray();
        while (!stoppingToken.IsCancellationRequested)
        {
            await ReadAll();
            await Task.Delay(_runId != null ? SAMPLE_DELAY_HIGH : SAMPLE_DELAY_LOW);
        }
    }

    public async Task<Run> StartRun()
    {
        if (_runId != null)
        {
            throw new Exception("Already running");
        }
        var configuration = await _configurationService.GetActiveConfiguration();
        using var context = new DataContext();
        var run = new Run()
        {
            StartTime = DateTime.Now,
            ConfigurationId = configuration.Id
        };
        context.Runs.Add(run);
        await context.SaveChangesAsync();
        _runId = run.Id;

        await context.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint;");
        
        Console.WriteLine($"{DateTime.Now}: Started run");
        await PublishDataState();

        return run;
    }

    public async Task<Run> StopRun()
    {
        // Stop logging
        var oldRunId = _runId;
        _runId = null;

        // Save run end
        using var context = new DataContext();
        var run = await context.Runs.FindAsync(oldRunId) ?? throw new Exception("Configuration ID did not exist");
        run.EndTime = DateTime.Now;
        await context.SaveChangesAsync();
        
        await context.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint;");
        Console.WriteLine($"{DateTime.Now}: Stopped run");

        await PublishDataState();
        return run;
    }

    private async Task PublishDataState()
    {
        _dataState.RunId = _runId;
        await _dataNotifier.PublishDataState(_dataState);
        // Console.WriteLine($"{DateTime.Now}: Published data state");
    }


}