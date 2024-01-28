using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.Models;

public class Configuration
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Parameter> Parameters { get; set; } = [];
    public ICollection<Device> Devices { get; set; } = [];
}