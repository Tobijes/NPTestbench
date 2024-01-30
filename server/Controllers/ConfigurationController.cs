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

    [HttpGet()]
    public async Task<Configuration> GetActiveConfiguration()
    {
        var configuration = await _configurationService.GetActiveConfiguration();
        return configuration;
    }

    [HttpGet("{id}")]
    public async Task<Configuration> GetConfiguration(int id)
    {
        var configuration = await _configurationService.GetById(id);
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
        public required string Name { get; set; }
    }
    [HttpPost]
    public async Task<Configuration> CreateConfiguration(CreateConfigurationRequest input)
    {
        var configuration = await _configurationService.Create(input.Name);
        return configuration;
    }

    public class AddParameterRequest
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

    [HttpPost("{configurationId}/Parameter")]
    public Task<Configuration> AddParameter(int configurationId, AddParameterRequest input)
    {
        var configuration = _configurationService.AddParameter(configurationId, input.Name, input.Value);
        return configuration;
    }


    public class AddDeviceRequest
    {
        public required string Name { get; set; }
        public required ushort StartAddress { get; set; }
        public required DeviceDataType DataType { get; set; }
        public string? DrawingID { get; set; }
    }

    [HttpPost("{configurationId}/Device")]
    public Task<Configuration> AddParameter(int configurationId, AddDeviceRequest input)
    {
        var configuration = _configurationService.AddDevice(configurationId, input.Name, input.StartAddress, input.DataType, input.DrawingID);
        return configuration;
    }

}