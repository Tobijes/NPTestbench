using NPTestbench.Models;

public class Channel {
    public int Id { get; set; }
    public string Name { get; set; }
    public ushort Address { get; set; }
    public bool Writable { get; set; }
    public DeviceDataType DataType { get; set;}
}