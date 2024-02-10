using NPTestbench.Models;

public class DataService : BackgroundService, IDisposable
{

    private readonly ConfigurationService _configurationService;
    private readonly DataNotifier _dataNotifier;
    private readonly CommunicationService _communcationService;

    private const int SAMPLE_DELAY_LOW = 1000 / 1;
    private const int SAMPLE_DELAY_HIGH = 1000 / 3;
    private int? _runId;

    public DataService(ConfigurationService configurationService, DataNotifier dataNotifier, CommunicationService communicationService)
    {
        _configurationService = configurationService;
        _dataNotifier = dataNotifier;
        _communcationService = communicationService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Device[] devices = (await _configurationService.GetActiveConfiguration()).Devices.ToArray();
        int? lastRunId = null;


        while (!stoppingToken.IsCancellationRequested)
        {
            if (_runId != null && lastRunId == null)
            { // Run Started
                // Refresh devices (make event-based instead)
                devices = (await _configurationService.GetActiveConfiguration()).Devices.ToArray();
            }

            float[] values = await _communcationService.ReadDevices(devices);

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
                await _dataNotifier.PublishMessage(new DataNotifier.DataMessage() { 
                    DeviceId = devices[i].Id, Value = values[i], DrawingId = devices[i].DrawingID
                });
                Console.WriteLine($"Device: {devices[i].Name} ({devices[i].DrawingID}) Value: {values[i]}");
            }


            await Task.Delay(_runId != null ? SAMPLE_DELAY_HIGH : SAMPLE_DELAY_LOW);
        }
    }

    public async Task<Run> StartRun()
    {
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
        return run;
    }


}