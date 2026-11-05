using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdontoClass.Data.ResultPattern.Extensions;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;
using OdontoClass.Services.Interfaces;
using OdontoClass.Utils;

namespace OdontoClass.Controllers;

[ApiController]
[Route("Schedule")]
[Authorize]
public class ScheduleController(IScheduleService _service) : ControllerBase
{
    
    [HttpGet("Schedules")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetSchedulesResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetSchedulesAsync([FromQuery] GetSchedulesRequest request)
    {
        return (await _service.GetSchedulesAsync(request)).ToIActionResult(this);
    }
    
    [HttpPost("Schedules")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> PostSchedulesAsync([FromBody] List<PostSchedulesRequest> request)
    {
        return (await _service.PostSchedulesAsync(request)).ToIActionResult(this);
    }
}
