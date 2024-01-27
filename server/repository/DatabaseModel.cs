using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPTestbench.repository;
public class DatabaseModel : DbContext
{
    public DbSet<Parameter> Parameters { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Run> Runs { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       optionsBuilder.UseSqlite("Data Source=Data.db");
   }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set up primary keys
        modelBuilder.Entity<Parameter>().HasKey(p => p.ParameterID);
        modelBuilder.Entity<Configuration>().HasKey(c => c.UniqueID);
        modelBuilder.Entity<Run>().HasKey(r => r.UniqueID);
        modelBuilder.Entity<Device>().HasKey(d => d.UniqueID);
        modelBuilder.Entity<Measurement>().HasKey(m => m.UniqueID);

        // Set up relationships
        modelBuilder.Entity<Parameter>()
            .HasOne<Configuration>()
            .WithMany()
            .HasForeignKey(p => p.ConfigurationID);

        modelBuilder.Entity<Device>()
            .HasOne<Configuration>()
            .WithMany()
            .HasForeignKey(d => d.ConfigurationID);

        modelBuilder.Entity<Run>()
            .HasOne<Configuration>()
            .WithMany()
            .HasForeignKey(r => r.ConfigurationID);

        modelBuilder.Entity<Measurement>()
            .HasOne<Run>()
            .WithMany()
            .HasForeignKey(m => m.RunID);

        modelBuilder.Entity<Measurement>()
            .HasOne<Device>()
            .WithMany()
            .HasForeignKey(m => m.DeviceID);
    }
}

public class Parameter
{
    public int ParameterID { get; set; }
    public int ConfigurationID { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}

public class Configuration
{
    public int UniqueID { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class Run
{
    public int UniqueID { get; set; }
    public int ConfigurationID { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; } // Nullable if the run can be ongoing
}

public class Device
{
    public int UniqueID { get; set; }
    public int ConfigurationID { get; set; }
    public string Name { get; set; }
    public string ProtocolID { get; set; } // Assuming this is a string identifier
    public string DrawingID { get; set; } // Assuming this is a string identifier
}

public class Measurement
{
    public int UniqueID { get; set; }
    public int RunID { get; set; }
    public int DeviceID { get; set; }
    public DateTime Timestamp { get; set; }
    public float Value { get; set; }
}
