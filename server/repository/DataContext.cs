using Microsoft.EntityFrameworkCore;

namespace NPTestbench.Models;
public class DataContext : DbContext
{
    public DbSet<Parameter> Parameters { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Run> Runs { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<DeviceChannel> DeviceChannels { get; set; }
    public DbSet<CalibratedValue> CalibratedValues { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       optionsBuilder.UseSqlite("Data Source=../data/Data.db");
   }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeviceChannel>()
            .HasKey(dc => new { dc.DeviceId, dc.ChannelId, dc.IsRead, dc.Order});
    }

}