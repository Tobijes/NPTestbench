using System.Net.Sockets;
using NModbus;
using NModbus.Device;
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
        Connect();
    }

    private void Connect()
    {
        try
        {
            var ip = "192.168.2.180";
            var port = 502;
            _client = new TcpClient(ip, port);
            var factory = new ModbusFactory();
            _master = factory.CreateMaster(_client);
            Console.WriteLine($"Connected to {ip}:{port}");
        }
        catch (Exception e)
        {
            Console.WriteLine("TCP client failed to connect");
        }
    }

    public void Reconnect() {
        Console.WriteLine("Reconnecting...");
        Connect();
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

    public Task<ByteValue> ReadChannel(Channel channel)
    {
         if (channel.Writable)
            {
                return ReadHoldingRegister(channel);
            }
            else
            {
                return ReadInputRegister(channel);
            }
    }

    public async Task<Dictionary<int, ByteValue>> ReadDevices(Device[] devices)
    {
        var tasks = new Dictionary<int, Task<ByteValue>>();
        for (int i = 0; i < devices.Length; i++)
        {
            var channels = devices[i].DeviceChannels
                .Where(dc => dc.IsRead)
                .Select(dc => dc.Channel);

            foreach (var channel in channels)
            {
                if (tasks.ContainsKey(channel.Id))
                {
                    continue;
                }
                tasks.Add(channel.Id, ReadChannel(channel));
            }
        }

        // Wait concurrently
        await Task.WhenAll(tasks.Values);

        var results = new Dictionary<int, ByteValue>();
        Random rnd = new Random();

        foreach (KeyValuePair<int, Task<ByteValue>> kv in tasks)
        {
            var result = await kv.Value;
         //   result *= rnd.Next(1, 15); // Add noise
            results.Add(kv.Key, result);
        }

        return results;
    }

    public async Task WriteChannel(Channel channel, ushort value)
    {
        // Only handle devices with writable addresses
        if (!channel.Writable)
        {
            return;
        }

        if (channel.DataType == DeviceDataType.Bit)
        {
            await _master.WriteSingleCoilAsync(1, channel.Address, value > 0);
        }
        else
        {
            await _master.WriteSingleRegisterAsync(1, channel.Address, value);
        }
    }

    private byte[] ConvertUShortsToBytes(ushort[] ushorts)
    {   
        // The individual ushorts with .GetBytes() get correct endianness
        // Windows is little-endian, Linux is big-endian
        if (ushorts.Length == 1) {
            return BitConverter.GetBytes(ushorts[0]);
        }

        byte[] bytes = new byte[ushorts.Length * 2];

        /*if (BitConverter.IsLittleEndian) 
        {
            // Flip endianess for pairs of bytes
            Array.Reverse(ushorts);
        }*/
        for (int i = 0; i < ushorts.Length; i++)
        {
            var bs = BitConverter.GetBytes(ushorts[i]);

            bytes[i * 2] = bs[1];
            bytes[i * 2 + 1] = bs[0];
        }
        if (BitConverter.IsLittleEndian)
        {
            // Flip endianess for all 4 bytes
            Array.Reverse(bytes);
        }
        
        return bytes;
    }


    private async Task<ByteValue> ReadInputRegister(Channel channel)
    {
        ushort numberOfPoints = NumberOfWords(channel.DataType);
        var words = await _master.ReadInputRegistersAsync(1, channel.Address, numberOfPoints);
        var bytes = ConvertUShortsToBytes(words);
        return new ByteValue(channel.DataType, bytes);
    }
    private async Task<ByteValue> ReadHoldingRegister(Channel channel)
    {
        ushort numberOfPoints = NumberOfWords(channel.DataType);
        var words = await _master.ReadHoldingRegistersAsync(1, channel.Address, numberOfPoints);
        var bytes = ConvertUShortsToBytes(words);
        return new ByteValue(channel.DataType, bytes);
    }


}