using Microsoft.AspNetCore.Mvc;

namespace NPTestbench.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{

    private DataService _dataService;
    public CommandController(DataService dataService) {
        _dataService = dataService;
    }

    [HttpPost]
    public string Valve([FromBody] bool open)
    {
        return "This is the Welcome action method...";
    }

}