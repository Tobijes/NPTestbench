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

            var parm1 = new Parameter()
            {
                Name = "DefaultParm",
                Value = "DefaulVal",
                ConfigurationId = defaultConfiguration.Id,
            };

            var parm2 = new Parameter()
            {
                Name = "DefaultParm2",
                Value = "DefaulVal2",
                ConfigurationId = defaultConfiguration.Id,
            };


            var device = new Device()
            {
                Name = "DefaultDevice1",
                StartAddress = 0,
                DataType = DeviceDataType.Float32,
                DrawingID = "Temp1",
                ConfigurationId = defaultConfiguration.Id
            };

            var device2 = new Device()
            {
                Name = "DefaultDevice2",
                StartAddress = 4,
                DataType = DeviceDataType.Float32,
                DrawingID = "Pres1",
                ConfigurationId = defaultConfiguration.Id
            };

            var valve1 = new Device()
            {
                Name = "Valve 1",
                StartAddress = 8,
                DataType = DeviceDataType.UInt16,
                ConfigurationId = defaultConfiguration.Id
            };

            var valve2 = new Device()
            {
                Name = "Valve 2",
                StartAddress = 10,
                DataType = DeviceDataType.UInt16,
                DrawingID = "Pres1",
                ConfigurationId = defaultConfiguration.Id
            };
            context.Parameters.AddRange([parm1, parm2]);
            context.Devices.Add(device);
            context.Devices.Add(device2);
            context.Devices.Add(valve1);
            context.Devices.Add(valve2);

            context.SaveChanges();
        }

        _activeConfiguration = context.Configurations.FirstOrDefault()!;
    }

    public async Task<Configuration> GetById(int id)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == id) ?? throw new Exception("Configuration ID did not exist");
        return configuration;
    }

    public async Task SetActiveConfig(int id)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == id) ?? throw new Exception("Configuration ID did not exist");

        _activeConfiguration = configuration;
        System.Console.WriteLine("active config is now: " + _activeConfiguration.Id);

    }

    public async Task<Configuration> GetActiveConfiguration()
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == _activeConfiguration.Id) ?? throw new Exception("Configuration ID did not exist");
        return configuration;
    }

    public async Task<List<Configuration>> List(int size = 25)
    {
        await using var context = new DataContext();
        var items = await context.Configurations
            .OrderBy(record => record.CreatedAt)
            .Take(size)
            .ToListAsync();
        return items;
    }

    public async Task<Configuration> Create(String configurationName)
    {
        await using var context = new DataContext();
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
        await using var context = new DataContext();
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


    public async Task UpdateParemter(int configurationId, int Id, string name, string value)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations.FindAsync(configurationId) ?? throw new Exception("Configuration ID did not exist");
        var parameter = configuration.Parameters.FirstOrDefault(param => param.Id == Id) ?? throw new Exception("paramter not found");
        parameter.Name = name;
        parameter.Value = value;
        await context.SaveChangesAsync();
    }

    public async Task DeleteParameter(int parameterId)
    {
        await using var context = new DataContext();
        var parameter = new Parameter { Id = parameterId };

        // Attach the stub entity to the context
        context.Parameters.Attach(parameter);

        context.Parameters.Remove(parameter);
        await context.SaveChangesAsync();
    }
    public async Task<Configuration> AddDevice(int configurationId, string name, ushort startAddress, DeviceDataType dataType, string? DrawingID)
    {
        await using var context = new DataContext();
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