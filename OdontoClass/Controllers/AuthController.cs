using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OdontoClass.Data.ResultPattern.Extensions;
using OdontoClass.DTOs.Requests.Auth;
using OdontoClass.DTOs.Responses.Auth;
using OdontoClass.Models;
using OdontoClass.Services.Interfaces;
using OdontoClass.Utils;
using System.Text.Encodings.Web;

namespace OdontoClass.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        return (await _authService.RegisterAsync(request)).ToIActionResult(this);
    }

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        return (await _authService.ConfirmEmailAsync(request)).ToIActionResult(this);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenrResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        return (await _authService.LoginAsync(request)).ToIActionResult(this);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> MeAsync()
    {
        return (await _authService.MeAsync()).ToIActionResult(this);
    }

    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetailsOdontoClass))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        return (await _authService.ChangePasswordAsync(request)).ToIActionResult(this);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return (await _authService.ForgotPasswordAsync(request)).ToIActionResult(this);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return (await _authService.ResetPasswordAsync(request)).ToIActionResult(this);
    }
}
