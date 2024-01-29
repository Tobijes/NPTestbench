using Microsoft.AspNetCore.Mvc;
using NPTestbench.Models;

namespace NPTestbench.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{

    private ConfigurationService _configurationService;

    public ConfigurationController(ConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [HttpGet("{id}")]
    public async Task<Configuration> GetConfiguration(int id)
    {
        var configuration = await _configurationService.Get(id);
        return configuration;
    }

    [HttpGet("List")]
    public async Task<List<Configuration>> ListConfigurations()
    {
        var configurations = await _configurationService.List();
        return configurations;
    }

    public class CreateConfigurationRequest
    {
        public string Name { get; set; }
    }
    [HttpPost]
    public async Task<Configuration> CreateConfiguration(CreateConfigurationRequest input)
    {
        var configuration = await _configurationService.Create(input.Name);
        return configuration;
    }

    public class AddParameterRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [HttpPost("{configurationId}/Parameter")]
    public Task<Configuration> AddParameter(int configurationId, AddParameterRequest input)
    {
        var configuration = _configurationService.AddParameter(configurationId, input.Name, input.Value);
        return configuration;
    }

}