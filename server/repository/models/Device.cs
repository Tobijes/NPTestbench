using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.Models;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProtocolID { get; set; } // Assuming this is a string identifier
    public string DrawingID { get; set; } // Assuming this is a string identifier

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }

    public ICollection<Measurement> Measurements { get; set; }
}