using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.Models;

public enum DeviceDataType {
    Bit = 0,
    Int16 = 1,
    UInt16 = 2,
    Int32 = 3,
    UInt32 = 4,
    Int64 = 5,
    UInt64 = 6,
    Float32 = 7,
}

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CalibrationFunctionName { get; set; } // Assuming this is a string identifier
    public string? DrawingID { get; set; } // Assuming this is a string identifier

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }

    public ICollection<Measurement> Measurements { get; set; }

    // Relations
    public ICollection<DeviceChannel> DeviceChannels { get; set; }
}