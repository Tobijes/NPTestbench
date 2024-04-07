using Microsoft.EntityFrameworkCore;
using NPTestbench.Models;
using NPTestbench.Service.Templates;

public delegate void Notify();  // delegate
public class ConfigurationService
{

    private Configuration _activeConfiguration;
    public event Notify ActiveConfigurationChanged; // event

    public ConfigurationService()
    {
        using var context = new DataContext();

        var defaultConfiguration = context.Configurations.FirstOrDefault();
        if (defaultConfiguration == null)
        {
            BootstrapDatabase();
        }
        _activeConfiguration = context.Configurations.FirstOrDefault()!;
        ActiveConfigurationChanged?.Invoke();
    }

    private void BootstrapDatabase() {
        using var context = new DataContext();

        var templateChannels = ConfigurationTemplateFactory.GenerateTemplateChannels();
        context.Channels.AddRange(templateChannels);
        context.SaveChanges();
        
        foreach (KeyValuePair<ConfigurationTemplateType, string> entry in ConfigurationTemplateFactory.DefaultNames) {
            var configuration = new Configuration()
            {
                Name = entry.Value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            context.Configurations.Add(configuration);
            context.SaveChanges();

            var templateParameters = ConfigurationTemplateFactory.GenerateTemplateParameters(entry.Key, configuration.Id);
            context.Parameters.AddRange(templateParameters);

            var templateDevices = ConfigurationTemplateFactory.GenerateTemplateDevices(entry.Key, configuration.Id);
            context.Devices.AddRange(templateDevices);

            context.SaveChanges();            

            var templateDeviceChannels = ConfigurationTemplateFactory.GenerateTemplateDeviceChannels(entry.Key, templateDevices, templateChannels);
            context.DeviceChannels.AddRange(templateDeviceChannels);

            context.SaveChanges();
        }
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

    public async Task<Configuration> SetActiveConfig(int id)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == id) ?? throw new Exception("Configuration ID did not exist");

        _activeConfiguration = configuration;
        ActiveConfigurationChanged?.Invoke();
        Console.WriteLine("active config is now: " + _activeConfiguration.Id);
        return _activeConfiguration;
    }

    public async Task<Configuration> GetActiveConfiguration()
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .ThenInclude(device => device.DeviceChannels)
            .ThenInclude(deviceChannel => deviceChannel.Channel)
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

    public async Task<Configuration> Rename(int configurationId, string name)
    {
        await using var context = new DataContext();
           var configuration = await context.Configurations
            .Include(configuration => configuration.Parameters)
            .Include(configuration => configuration.Devices)
            .FirstAsync(c => c.Id == configurationId) ?? throw new Exception("Configuration ID did not exist");
        configuration.Name = name;
        await context.SaveChangesAsync();
        return configuration;
    }

    public async Task<Configuration> Clone(int configurationId)
    {
        await using var context = new DataContext();
        var oldConfiguration = await GetById(configurationId);

        var newConfiguration = new Configuration()
        {
            Name = oldConfiguration.Name + " - Copy",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

        context.Configurations.Add(newConfiguration);
        await context.SaveChangesAsync();
        
        // Clone parameters (without relation)
        foreach (var parameter in  oldConfiguration.Parameters)
        {
            context.Parameters.Add(new Parameter()
            {
                Name = parameter.Name,
                Value = parameter.Value,
                ConfigurationId = newConfiguration.Id
            });
        }

        // Clone devices (without relation)
        foreach (var device in oldConfiguration.Devices)
        {
            context.Devices.Add(new Device()
            {
                Name = device.Name,
                DrawingID = device.DrawingID,
                ConfigurationId = newConfiguration.Id
            });
        }

        // Clone device channels (without relation)
        foreach (var device in oldConfiguration.Devices)
        {   
            foreach (var deviceChannel in device.DeviceChannels) {
                context.DeviceChannels.Add(new DeviceChannel()
                {
                    DeviceId = deviceChannel.DeviceId,
                    ChannelId = deviceChannel.ChannelId,
                    IsRead = deviceChannel.IsRead,
                    Order = deviceChannel.Order
                });
            }
        }

        await context.SaveChangesAsync();
        return newConfiguration;
    }

    public async Task<Parameter> AddParameter(int configurationId, string name, string value)
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
        return parameter;
    }


    public async Task<Parameter> UpdateParameter(int configurationId, int Id, string name, string value)
    {
        await using var context = new DataContext();
        var configuration = await context.Configurations.Include(configuration => configuration.Parameters).FirstAsync(config => config.Id == configurationId) ?? throw new Exception("Configuration ID did not exist");
        var parameter = configuration.Parameters.FirstOrDefault(param => param.Id == Id) ?? throw new Exception("parameter not found");
        parameter.Name = name;
        parameter.Value = value;
        await context.SaveChangesAsync();

        return parameter;
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
}