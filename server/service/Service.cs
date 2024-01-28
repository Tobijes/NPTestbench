using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NModbus;
using NPTestbench.repository;


public class DataService : BackgroundService, IDisposable
{

    TcpClient _client;
    IModbusMaster _master;
    private readonly IHubContext<DataHub> _hubContext;

    public DataService(IHubContext<DataHub> hubContext)
    {
        _client = new TcpClient("127.0.0.1", 5020);
        var factory = new ModbusFactory();
        _master = factory.CreateMaster(_client);
        _hubContext = hubContext;
    }

    private byte[] CorrectEndian(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        // Console.WriteLine(string.Join(" ", bytes));
        return bytes;
    }
    private byte[] ConvertUShortsToBytes(ushort[] ushorts)
    {
        // Console.WriteLine(string.Join(" ", ushorts));
        byte[] bytes = new byte[ushorts.Length * 2];
        for (int i = 0; i < ushorts.Length; i++)
        {
            var bs = BitConverter.GetBytes(ushorts[i]);
            bytes[i * 2] = bs[0];
            bytes[i * 2 + 1] = bs[1];
        }
        // Console.WriteLine(string.Join(" ", bytes));
        return bytes;
    }

    public async Task<float> ReadValues()
    {
        var words = await _master.ReadHoldingRegistersAsync(1, 0, 2);
        var bytes = ConvertUShortsToBytes(words);
        bytes = CorrectEndian(bytes);
        return BitConverter.ToSingle(bytes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int runId = 0;
        int deviceId = 0;
        using (var context = new DataContext())
        {
            context.Database.EnsureCreated();
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var configuration = new Configuration()
            {
                Name = "MyName",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            context.Configurations.Add(configuration);
            context.SaveChanges();

            var run = new Run()
            {
                ConfigurationId = configuration.Id,
                StartTime = DateTime.Now
            };
            context.Runs.Add(run);
            context.SaveChanges();

            var device = new Device()
            {
                Name = "MyDevice",
                ProtocolID = "MyProtocol",
                DrawingID = "Sensor1",
                ConfigurationId = configuration.Id
            };
            context.Devices.Add(device);
            context.SaveChanges();

            runId = run.Id;
            deviceId = device.Id;
        }



        while (!stoppingToken.IsCancellationRequested)
        {
            float value = await ReadValues();
            float faked = value * (DateTime.UtcNow.Millisecond % 5);

            var measurement = new Measurement()
            {
                DeviceId = deviceId,
                RunId = runId,
                Timestamp = DateTime.Now,
                Value = faked
            };

            using (var context = new DataContext())
            {
                context.Measurements.Add(measurement);
                context.SaveChanges();
            }

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Value: {measurement.Value}");
            Console.WriteLine($"Real: {value}, Faked: {faked}");
            await Task.Delay(1000);
        }
    }


}