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
        await _communicationService.WriteDevice(device, value);
        await ReadAll();
    }

    public async Task ReadAll()
    {
        float[] values = await _communicationService.ReadDevices(devices);

        // Only save to log if we are running
        if (_runId != null)
        {
            DateTime timestamp = DateTime.Now;
            using (var context = new DataContext())
            {
                for (int i = 0; i < devices.Length; i++)
                {
                    var measurement = new Measurement()
                    {
                        DeviceId = devices[i].Id,
                        RunId = (int)_runId,
                        Timestamp = timestamp,
                        Value = values[i]
                    };
                    context.Measurements.Add(measurement);
                }
                context.SaveChanges();
            }
        }

        for (int i = 0; i < devices.Length; i++)
        {
            int id = devices[i].Id;
            if (!_dataState.DeviceStates.ContainsKey(id))
            {
                _dataState.DeviceStates.Add(id, new DataNotifier.DeviceState()
                {
                    Id = id,
                    Name = devices[i].Name,
                    DrawingId = devices[i].DrawingID,
                });
            }

            _dataState.DeviceStates[id].Value = values[i];
            _dataState.DeviceStates[id].ValueRunMaximum = Math.Max(_dataState.DeviceStates[id].ValueRunMaximum, values[i]);
            _dataState.DeviceStates[id].ValueRunMinimum = Math.Min(_dataState.DeviceStates[id].ValueRunMinimum, values[i]);
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