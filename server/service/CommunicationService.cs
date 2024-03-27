using System.Net.Sockets;
using NModbus;
using NPTestbench.Models;

/*
Coil (discrete output)  Read-write	1 bit	Read/Write on/off value
Discrete input	        Read-only	1 bit	Read on/off value
Input register	        Read-only	16 bits (0–65,535)	Read measurements and statuses
Holding register	    Read-write	16 bits (0–65,535)	Read/Write configuration values
*/

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
        DeviceDataType.Bit => 1,
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

        var tasks = new Task<float>[devices.Length];
        for (int i = 0; i < devices.Length; i++)
        {
            Device device = devices[i];

            if (device.DataType == DeviceDataType.Bit)
            {
                if (device.WriteAddress == null)
                {
                    tasks[i] = ReadDiscreteInput(device);
                }
                else
                {
                    tasks[i] = ReadCoil(device);
                }
            }
            else
            {
                if (device.WriteAddress == null)
                {
                    tasks[i] = ReadInputRegister(device);
                }
                else
                {
                    tasks[i] = ReadHoldingRegister(device);
                }
            }
        }

        // Wait concurrently
        await Task.WhenAll(tasks);
        for (int i = 0; i < tasks.Length; i++)
        {
            // Get result from tasks (we know it's already done)
            results[i] = await tasks[i];
        }

        // Add noise
        Random rnd = new Random();
        for (int i = 0; i < tasks.Length; i++)
        {
            results[i] = results[i] * rnd.Next(1,15);
        }

        return results;
    }

    public async Task WriteDevice(Device device, ushort value)
    {   
        // Only handle devices with writable addresses
        if (device.WriteAddress == null) {
            return;
        }

        if (device.DataType == DeviceDataType.Bit) {
            await WriteCoil(device, value);
        }  else {
            await WriteHoldingRegister(device, value);
        }
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
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    private async Task WriteCoil(Device device, ushort value)
    {
         if (device.WriteAddress == null) {
            return;
        }
        await _master.WriteSingleCoilAsync(1, (ushort) device.WriteAddress, value > 0);
    }

    private async Task WriteHoldingRegister(Device device, ushort value)
    {
         if (device.WriteAddress == null) {
            return;
        }
        await _master.WriteSingleRegisterAsync(1, (ushort) device.WriteAddress, value);
    }

    private async Task<float> ReadCoil(Device device)
    {
        ushort numberOfPoints = NumberOfWords(device.DataType);
        var bits = await _master.ReadCoilsAsync(1, device.ReadAddress, numberOfPoints);
        return bits[0] ? 1 : 0;
    }

    private async Task<float> ReadDiscreteInput(Device device)
    {
        ushort numberOfPoints = NumberOfWords(device.DataType);
        var bits = await _master.ReadInputsAsync(1, device.ReadAddress, numberOfPoints);
        return bits[0] ? 1 : 0;
    }

    private async Task<float> ReadInputRegister(Device device)
    {
        ushort numberOfPoints = NumberOfWords(device.DataType);
        var words = await _master.ReadInputRegistersAsync(1, device.ReadAddress, numberOfPoints);
        var bytes = ConvertUShortsToBytes(words);
        float value = ConvertBytes(device.DataType, bytes);
        return value;
    }

    private async Task<float> ReadHoldingRegister(Device device)
    {
        ushort numberOfPoints = NumberOfWords(device.DataType);
        var words = await _master.ReadHoldingRegistersAsync(1, device.ReadAddress, numberOfPoints);
        var bytes = ConvertUShortsToBytes(words);
        float value = ConvertBytes(device.DataType, bytes);
        return value;
    }


}