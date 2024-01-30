using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.Models;

public enum DeviceDataType {
    Int16 = 0,
    UInt16 = 1,
    Int32 = 2,
    UInt32 = 3,
    Int64 = 4,
    UInt64 = 5,
    Float32 = 6,
}

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ushort StartAddress { get; set;}
    public DeviceDataType DataType { get; set;}
    public string? DrawingID { get; set; } // Assuming this is a string identifier

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }

    public ICollection<Measurement> Measurements { get; set; }
}