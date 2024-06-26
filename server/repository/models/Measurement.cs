using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.Models;

public class Measurement
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public float Value { get; set; }

    public int RunId { get; set; }
    public Run Run { get; set; }

    public int ChannelId { get; set; }
    public Channel Channel { get; set; }
}