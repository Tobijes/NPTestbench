using Microsoft.EntityFrameworkCore;
using NPTestbench.Models;

public class ConfigurationService
{

    private Configuration _activeConfiguration;

    public ConfigurationService()
    {
        using var context = new DataContext();
        
        var defaultConfiguration = context.Configurations.FirstOrDefault();
        if (defaultConfiguration == null)
        {
            defaultConfiguration = new Configuration()
            {
                Name = "Default",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            context.Configurations.Add(defaultConfiguration);
            context.SaveChanges();
        }

        _activeConfiguration = defaultConfiguration;
    }

    public async Task<Configuration> GetById(int id)
    {
        using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == id) ?? throw new Exception("Configuration ID did not exist");
        return configuration;
    }

    public async Task<Configuration> GetActiveConfiguration()
    {   
        using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == _activeConfiguration.Id) ?? throw new Exception("Configuration ID did not exist");
        return configuration;
    }

    public async Task<List<Configuration>> List(int size = 25)
    {
        using var context = new DataContext();
        var items = await context.Configurations
            .OrderBy(record => record.CreatedAt)
            .Take(size)
            .ToListAsync();
        return items;
    }

    public async Task<Configuration> Create(String configurationName)
    {
        using var context = new DataContext();
        var configuration = new Configuration()
        {
            Name = configurationName,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        context.Configurations.Add(configuration);
        await context.SaveChangesAsync();
        return configuration;
    }

    public async Task<Configuration> AddParameter(int configurationId, string name, string value)
    {
        using var context = new DataContext();
        var configuration = await context.Configurations.FindAsync(configurationId) ?? throw new Exception("Configuration ID did not exist");
        var parameter = new Parameter()
        {
            Name = name,
            Value = value
        };
        configuration.Parameters.Add(parameter);
        await context.SaveChangesAsync();
        return configuration;
    }

    public async Task<Configuration> AddDevice(int configurationId, string name, ushort startAddress, DeviceDataType dataType, string? DrawingID)
    {
        using var context = new DataContext();
        var configuration = await context.Configurations.FindAsync(configurationId) ?? throw new Exception("Configuration ID did not exist");
        var device = new Device()
        {
            Name = name,
            StartAddress = startAddress,
            DataType = dataType,
            DrawingID = DrawingID
        };
        configuration.Devices.Add(device);
        await context.SaveChangesAsync();
        return configuration;
    }
}