using Microsoft.EntityFrameworkCore;

namespace NPTestbench.repository;
public class DataContext : DbContext
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

}