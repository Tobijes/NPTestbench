using Microsoft.AspNetCore.Mvc;

namespace NPTestbench.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{

    private ConfigurationService _configurationService;
    private DataService _dataService;

    private const ushort OPEN = ushort.MaxValue; 
    private const ushort CLOSED = ushort.MinValue;

    public CommandController(ConfigurationService configurationService, DataService dataService) {
        _configurationService = configurationService;
        _dataService = dataService;
    }

    [HttpPost("Open/{deviceId}")]
    public async Task<ActionResult> Open(int deviceId)
    {
        var configuration = await _configurationService.GetActiveConfiguration();
        var device = configuration.Devices.Where(device =>  device.Id == deviceId).FirstOrDefault();
        if (device == null) {
            return NotFound("Device not found by ID");
        }
        await _dataService.WriteDevice(device, OPEN);
        return Ok();
    }

    [HttpPost("Close/{deviceId}")]
    public async Task<ActionResult> Close(int deviceId)
    {
        var configuration = await _configurationService.GetActiveConfiguration();
        var device = configuration.Devices.Where(device =>  device.Id == deviceId).FirstOrDefault();
        if (device == null) {
            return NotFound("Device not found by ID");
        }
        await _dataService.WriteDevice(device, CLOSED);
        return Ok();
    }

    [HttpPost("Pulse/{deviceId}")]
    public async Task<ActionResult> Pulse(int deviceId)
    {
        var configuration = await _configurationService.GetActiveConfiguration();
        var device = configuration.Devices.Where(device =>  device.Id == deviceId).FirstOrDefault();
        if (device == null) {
            return NotFound("Device not found by ID");
        }

        var isOpen = true;
        var first = isOpen ? CLOSED : OPEN;
        var second = isOpen ? OPEN : CLOSED;
        await _dataService.WriteDevice(device, first);
        await Task.Delay(100);
        await _dataService.WriteDevice(device, second);
        return Ok();
    }

}