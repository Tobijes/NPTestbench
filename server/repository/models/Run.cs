using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.repository;

public class Run
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; } // Nullable if the run can be ongoing

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }

    public ICollection<Measurement> Measurements { get; set; }
}