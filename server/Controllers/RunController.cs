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
    public  Task<Run> Start()
    {
        return  _dataService.StartRun();
    }

    [HttpPost("Stop")]
    public  Task<Run> Stop()
    {
        return  _dataService.StopRun();
    }

}