using Microsoft.EntityFrameworkCore;
using NPTestbench.Models;
using NPTestbench.Service.Templates;

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
                Name = "Default: Stage 1 and 2, vacuum chamber",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            context.Configurations.Add(defaultConfiguration);
            context.SaveChanges();

            var templateParameters = ConfigurationTemplateFactory.GenerateTemplateParameters(ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, defaultConfiguration.Id);
            context.Parameters.AddRange(templateParameters);
            
            var templateDevices = ConfigurationTemplateFactory.GenerateTemplateDevices(ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, defaultConfiguration.Id);
            context.Devices.AddRange(templateDevices);

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

    public async Task<Configuration> GetConfigurationByID(int id)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == id) ?? throw new Exception("Configuration ID did not exist");
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

    public async Task<int> AddParameter(int configurationId, string name, string value)
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
        return parameter.Id;
    }


    public async Task UpdateParemter(int configurationId, int Id, string name, string value)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations.Include(configuration => configuration.Parameters).FirstAsync(config => config.Id == configurationId) ?? throw new Exception("Configuration ID did not exist");
        var parameter = configuration.Parameters.FirstOrDefault(param => param.Id == Id) ?? throw new Exception("parameter not found");
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