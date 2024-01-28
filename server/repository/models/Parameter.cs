using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.repository;

public class Parameter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }
}