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
    public  Task<Configuration> GetConfiguration(int id)
    {
        var configuration =  _configurationService.GetById(id);
        return configuration;
    }

    [HttpGet()]
    public  Task<List<Configuration>> ListConfigurations()
    {
        var configurations =  _configurationService.List();
        return configurations;
    }

    [HttpGet("active")]
    public async Task<Configuration> GetActiveConfiguration()
    {
        var configuration = await _configurationService.GetActiveConfiguration();
        return configuration;
    }
    
    [HttpPost("active/{id}")]
    public async Task<Configuration> SetActiveConfiguration(int id)
    {
       var configuration = await _configurationService.SetActiveConfig(id);
       return configuration;
    }

    public class RenameConfigurationRequest
    {
        public required string Name { get; set; }
    }

    [HttpPost("{configurationId}/Rename")]
    public async Task<Configuration> RenameConfiguration(int configurationId, RenameConfigurationRequest input)
    {
        var configuration = await _configurationService.Rename(configurationId, input.Name);
        return configuration;
    }

    [HttpPost("{configurationId}/Clone")]
    public Task<Configuration> CloneConfiguration(int configurationId)
    {
        return _configurationService.Clone(configurationId);
    }

    public class AddParameterRequest
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

    [HttpPut("{configurationId}/Parameter")]
    public async Task<Parameter> AddParameter(int configurationId, AddParameterRequest input)
    {
        var parameter = await _configurationService.AddParameter(configurationId, input.Name, input.Value);
        return parameter;
    }

     public class UpdateParameterRequest
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

    [HttpPost("{configurationId}/Parameter/{parameterId}")]
    public async Task<Parameter> UpdateParameter(int configurationId, int parameterId, UpdateParameterRequest input)
    {
        var parameter = await _configurationService.UpdateParameter(configurationId, parameterId, input.Name, input.Value);
       return parameter;
    }

    [HttpDelete("{configurationId}/Parameter/{parameterId}")]
    public async Task DeleteParameter(int configurationId, int parameterId)
    {
         await _configurationService.DeleteParameter(parameterId);
    }

    public class AddDeviceRequest
    {
        public required string Name { get; set; }
        public required ushort StartAddress { get; set; }
        public required DeviceDataType DataType { get; set; }
        public string? DrawingID { get; set; }
    }

    [HttpPost("{configurationId}/Device")]
    public Task<Configuration> AddDevice(int configurationId, AddDeviceRequest input)
    {
        var configuration = _configurationService.AddDevice(configurationId, input.Name, input.StartAddress, input.DataType, input.DrawingID);
        return configuration;
    }

}