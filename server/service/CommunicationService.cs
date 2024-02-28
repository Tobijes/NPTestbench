using System.Net.Sockets;
using NModbus;
using NPTestbench.Models;

public class CommunicationService
{
    TcpClient _client;
    IModbusMaster _master;

    public CommunicationService()
    {
       
        try
        {
            _client = new TcpClient("127.0.0.1", 5020);
            var factory = new ModbusFactory();
            _master = factory.CreateMaster(_client);

        }
        catch (Exception e)
        {
            System.Console.WriteLine("TCP client failed to connect");
        }
    }

    ushort NumberOfWords(DeviceDataType dataType) => dataType switch
    {
        DeviceDataType.Int16 => 1,
        DeviceDataType.UInt16 => 1,
        DeviceDataType.Int32 => 2,
        DeviceDataType.UInt32 => 2,
        DeviceDataType.Int64 => 4,
        DeviceDataType.UInt64 => 4,
        DeviceDataType.Float32 => 2,
        _ => throw new NotImplementedException(),
    };

    float ConvertBytes(DeviceDataType dataType, byte[] bytes) => dataType switch
    {
        DeviceDataType.Int16 => BitConverter.ToInt16(bytes),
        DeviceDataType.UInt16 => BitConverter.ToUInt16(bytes),
        DeviceDataType.Int32 => BitConverter.ToInt32(bytes),
        DeviceDataType.UInt32 => BitConverter.ToUInt32(bytes),
        DeviceDataType.Int64 => BitConverter.ToInt64(bytes),
        DeviceDataType.UInt64 => BitConverter.ToUInt64(bytes),
        DeviceDataType.Float32 => BitConverter.ToSingle(bytes),
        _ => throw new NotImplementedException(),
    };

    public async Task<float[]> ReadDevices(Device[] devices)
    {
        var results = new float[devices.Length];

        for (int i = 0; i < devices.Length; i++)
        {
            Device device = devices[i];
            ushort numberOfPoints = NumberOfWords(device.DataType);
            var words = await _master.ReadHoldingRegistersAsync(1, device.StartAddress, numberOfPoints);
            var bytes = ConvertUShortsToBytes(words);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            float value = ConvertBytes(device.DataType, bytes);
            results[i] = value * (DateTime.UtcNow.Millisecond % 5);
        }

        return results;
    }

    public async Task WriteDevice(Device device, ushort value) {

        await _master.WriteSingleRegisterAsync(1, device.StartAddress, value);
    }

    private byte[] ConvertUShortsToBytes(ushort[] ushorts)
    {
        byte[] bytes = new byte[ushorts.Length * 2];
        for (int i = 0; i < ushorts.Length; i++)
        {
            var bs = BitConverter.GetBytes(ushorts[i]);
            bytes[i * 2] = bs[0];
            bytes[i * 2 + 1] = bs[1];
        }
        return bytes;
    }
}