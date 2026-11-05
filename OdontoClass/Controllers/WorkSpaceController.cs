using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdontoClass.Data.ResultPattern.Extensions;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.Services.Interfaces;
using OdontoClass.Utils;

namespace OdontoClass.Controllers;

[ApiController]
[Route("WorkSpace")]
[Authorize]
public class WorkSpaceController(IWorkSpaceService _service) : ControllerBase
{
    [HttpPost("WorkSpace")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> PostWorkSpacesAsync(List<PostWorkSpaceRequest> request)
    {
        return (await _service.PostWorkSpacesAsync(request)).ToIActionResult(this);
    }

    [HttpGet("WorkSpace")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetWorkSpacesAsync(GetWorkSpacesRequest request)
    {
        return (await _service.GetWorkSpacesAsync(request)).ToIActionResult(this);
    }
}
