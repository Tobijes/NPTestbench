using NPTestbench.Models;

public class ConfigurationService
{


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
        var configuration = await context.Configurations.FindAsync(configurationId);
        var parameter = new Parameter() {
            ConfigurationId = configuration.Id,
            Name = name,
            Value = value
        };
        context.Parameters.Add(parameter);
        await context.SaveChangesAsync();
        return configuration;
    }
}