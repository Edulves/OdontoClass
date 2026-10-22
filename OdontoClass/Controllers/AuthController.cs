using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using OdontoClass.DTOs.Requests;
using OdontoClass.Models;
using OdontoClass.Services;
using OdontoClass.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace OdontoClass.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration,
    IEmailService emailService)
    {
        _emailService = emailService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email.Trim(),
            Email = request.Email.Trim()
        };

        var result = await _userManager.CreateAsync( user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var apiUrl = _configuration["App:PublicApiUrl"] ?? throw new InvalidOperationException("A URL pública da API não foi configurada.");

        var confirmationUrl = QueryHelpers.AddQueryString(
            $"{apiUrl.TrimEnd('/')}/auth/confirm-email",
            new Dictionary<string, string?>
            {
            ["userId"] = user.Id.ToString(),
            ["token"] = encodedToken
            });

        var safeUrl = HtmlEncoder.Default.Encode(confirmationUrl);

        var htmlBody = $"""
            <h2>Bem-vindo ao OdontoClass!</h2>
            <p>Para confirmar seu endereço de e-mail, clique no link abaixo:</p>
            <p><a href="{safeUrl}">Confirmar meu e-mail</a></p>
            <p>Se você não criou essa conta, ignore esta mensagem.</p>
            """;

        await _emailService.SendAsync(
            user.Email!,
            "Confirme seu e-mail - OdontoClass",
            htmlBody);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Usuário cadastrado com sucesso."
        });
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(
    [FromQuery] int userId,
    [FromQuery] string token)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return BadRequest(new
            {
                message = "Link de confirmação inválido ou expirado."
            });
        }

        string decodedToken;

        try
        {
            decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return BadRequest(new
            {
                message = "Link de confirmação inválido ou expirado."
            });
        }

        var result = await _userManager.ConfirmEmailAsync(
            user,
            decodedToken);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = "Não foi possível confirmar o e-mail. O link pode estar inválido ou expirado."
            });
        }

        return Ok(new
        {
            message = "E-mail confirmado com sucesso. Você já pode entrar na sua conta."
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var email = request.Email.Trim();

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || user.IsDeleted)
        {
            return Unauthorized(new
            {
                message = "E-mail ou senha inválidos."
            });
        }

        var result = await _signInManager
            .CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                message = "E-mail ou senha inválidos."
            });
        }

        var token = await GenerateJwtTokenAsync(user);

        return Ok(new
        {
            accessToken = token,
            tokenType = "Bearer",
            expiresIn = _configuration.GetValue<int>(
                "Jwt:ExpiresMinutes") * 60
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null || user.IsDeleted)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            user.Id,
            user.Email,
            user.UserName,
            user.CreatedAt
        });
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null || user.IsDeleted)
        {
            return Unauthorized();
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });
        }

        return NoContent();
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "Chave JWT não configurada.");

        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;

        var expiresMinutes = _configuration.GetValue<int>(
            "Jwt:ExpiresMinutes");

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role =>
            new Claim(ClaimTypes.Role, role)));

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
