// Controllers/AccidentReportController.cs
using AcciCom.Models;
using AcciCom.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccidentReportController : ControllerBase
{
    private readonly IEventHubService _eventHubService;
    private readonly ILogger<AccidentReportController> _logger;

    public AccidentReportController(IEventHubService eventHubService, ILogger<AccidentReportController> logger)
    {
        _eventHubService = eventHubService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AccidentReport report)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Set timestamp just before sending
            report.Timestamp = DateTime.UtcNow;

            await _eventHubService.SendAccidentReportAsync(report);

            // Return 'Accepted' to indicate it's been queued for processing
            return Accepted(new { message = "Accident report received and queued for processing.", reportId = report.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending accident report.");
            return StatusCode(500, "An internal error occurred.");
        }
    }
}