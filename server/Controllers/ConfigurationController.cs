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
    public  Task<Configuration> GetActiveConfiguration()
    {
        var configuration =  _configurationService.GetActiveConfiguration();
        return configuration;
    }

    [HttpGet("{id}")]
    public  Task<Configuration> GetConfiguration(int id)
    {
        var configuration =  _configurationService.GetById(id);
        return configuration;
    }

    [HttpPost("SetActiveConfiguration/{id}")]
    public async Task SetActiveConfiguration(int id)
    {
       await _configurationService.SetActiveConfig(id);
    }

    [HttpGet("List")]
    public  Task<List<Configuration>> ListConfigurations()
    {
        var configurations =  _configurationService.List();
        return configurations;
    }

    [HttpGet("GetConfigById/{id}")]
    public Task<Configuration> GetConfigurationByID(int id)
    {
        return _configurationService.GetConfigurationByID(id);
    }

    public class CreateConfigurationRequest
    {
        public required string Name { get; set; }
    }

    [HttpPost("CreateConfiguration")]
    public Task<Configuration> CreateConfiguration(CreateConfigurationRequest input)
    {
        return _configurationService.Create(input.Name);
    }

    public class CloneConfigurationRequest
    {
        public required int Id { get; set; }
    }

    [HttpPost("Clone")]
    public Task<Configuration> CloneConfiguration(CloneConfigurationRequest input)
    {
        return _configurationService.Clone(input.Id);
    }

    public class AddParameterRequest
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

    public class UpdateParameterRequest
    {
        public required int Id {get; set;}
        public required string Name { get; set; }
        public required string Value { get; set; }
    }

  

    [HttpPost("{configurationId}/AddParameter")]
    public Task<int> AddParameter(int configurationId, AddParameterRequest input)
    {
        return  _configurationService.AddParameter(configurationId, input.Name, input.Value);
    }

    [HttpPost("{configurationId}/UpdateParameter")]
    public Task UpdateParameter(int configurationId, UpdateParameterRequest input)
    {
       return _configurationService.UpdateParameter(configurationId, input.Id, input.Name, input.Value);
    }

    [HttpPost("{parameterId}/DeleteParameter")]
    public async Task DeleteParameter(int parameterId)
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