using Microsoft.AspNetCore.Mvc;
using NPTestbench.Models;

namespace NPTestbench.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RunController : ControllerBase
{

    private DataService _dataService;
    public RunController(DataService dataService) {
        _dataService = dataService;
    }

    [HttpPost("Start")]
    public async Task<Run> Start()
    {
        return await _dataService.StartRun();
    }

    [HttpPost("Stop")]
    public async Task<Run> Stop()
    {
        return await _dataService.StopRun();
    }

}