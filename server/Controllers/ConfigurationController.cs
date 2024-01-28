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

        public int ConfigurationId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    // Does not work, creates cycle
    // [HttpPost("Parameter")]
    // public Task<Configuration> AddParameter(AddParameterRequest input)
    // {
    //     var configuration = _configurationService.AddParameter(input.ConfigurationId, input.Name, input.Value);
    //     return configuration;
    // }

}