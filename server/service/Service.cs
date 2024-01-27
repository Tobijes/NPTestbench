using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using NModbus;
using NPTestbench.repository;


public class Service : IDisposable
{

    TcpClient _client;
    IModbusMaster _master;

    public Service()
    {
        _client = new TcpClient("127.0.0.1", 5020);
        var factory = new ModbusFactory();
        _master = factory.CreateMaster(_client);
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

    public async Task StartReading(CancellationToken cancellationToken)
    {
        int runId = 0;
        int deviceId = 0;
        using (var context = new DatabaseModel())
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
                ConfigurationID = configuration.UniqueID,
                StartTime = DateTime.Now
            };
            context.Runs.Add(run);
            context.SaveChanges();

            var device = new Device()
            {
                Name = "MyDevice",
                ProtocolID = "MyProtocol",
                DrawingID = "Sensor1",
                ConfigurationID = configuration.UniqueID
            };
            context.Devices.Add(device);
            context.SaveChanges();

            runId = run.UniqueID;
            deviceId = device.UniqueID;
        }



        while (!cancellationToken.IsCancellationRequested)
        {
            float value = await ReadValues();
            float faked = value * (DateTime.UtcNow.Millisecond % 5);

            var measurement = new Measurement()
            {
                DeviceID = deviceId,
                RunID = runId,
                Timestamp = DateTime.Now,
                Value = faked
            };

            using (var context = new DatabaseModel())
            {
                context.Measurements.Add(measurement);
                context.SaveChanges();
            }

            Console.WriteLine($"Real: {value}, Faked: {faked}");
            await Task.Delay(1000);
        }
    }

    public void Dispose()
    {
        _client.Dispose();
        _master.Dispose();
    }



}