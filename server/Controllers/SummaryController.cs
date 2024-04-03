using Microsoft.AspNetCore.Mvc;
using NPTestbench.Models;

namespace NPTestbench.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SummaryController : ControllerBase
{

    private SummaryService _summaryService;
    public SummaryController(SummaryService summaryService) {
        _summaryService = summaryService;
    }

    [HttpGet("Summary")]
    public  Task<Summary> GetSummary()
    {
        return _summaryService.getLastSummary();
    }



}


