using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests;
using OdontoClass.DTOs.Responses;
using OdontoClass.Models;
using OdontoClass.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace OdontoClass.Services;

public class AuthService(UserManager<User> _userManager, SignInManager<User> _signInManager, IConfiguration _configuration, IEmailService _emailService) : IAuthService
{
    public async Task<ResultPattern<string>> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email.Trim(),
            Email = request.Email.Trim()
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return ResultPattern<string>.Failure(
                String.Join(Environment.NewLine, result.Errors.Select(e => $"- code={e.Code}, Description: {e.Description};")),"Algo deu errado ao tentar criar o novo usuário");

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

        return ResultPattern<string>.Success("Usuário cadastrado com sucesso.", StatusCodes.Status201Created);
    }

    public async Task<ResultPattern<string>> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
            return ResultPattern<string>.Failure("Link de confirmação inválido ou expirado.");

        string decodedToken;

        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        }
        catch (FormatException)
        {
            return ResultPattern<string>.Failure("Link de confirmação inválido ou expirado.");
        }

        var result = await _userManager.ConfirmEmailAsync(
            user,
            decodedToken);

        if (!result.Succeeded)
        {
            return ResultPattern<string>.Failure("Não foi possível confirmar o e-mail. O link pode estar inválido ou expirado.");
        }

        return ResultPattern<string>.Success("E-mail confirmado com sucesso. Você já pode entrar na sua conta.");
    }

    public async Task<ResultPattern<TokenrResponse>> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim();

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || user.IsDeleted)
            return ResultPattern<TokenrResponse>.Failure("E-mail ou senha inválidos.","Não autorizado", StatusCodes.Status401Unauthorized);

        var result = await _signInManager
            .CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: false);

        if (!result.Succeeded)
            return ResultPattern<TokenrResponse>.Failure("E-mail ou senha inválidos.", "Não autorizado", StatusCodes.Status401Unauthorized);


        var token = await GenerateJwtTokenAsync(user);

        var expiresIn = _configuration.GetValue<int>("Jwt:ExpiresMinutes") * 60;

        var tokenResult = new TokenrResponse(token, "Bearer", expiresIn);

        return ResultPattern<TokenrResponse>.Success(tokenResult);
    }

    public async Task<ResultPattern<MeResponse>> MeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || user.IsDeleted)
            return ResultPattern<MeResponse>.Failure("Não autorizado!", "Não autorizado!", StatusCodes.Status401Unauthorized);

        var result = new MeResponse(user.Id, user.Email ?? "", user.UserName ?? "", user.CreatedAt);

        return ResultPattern<MeResponse>.Success(result);
    }

    public async Task<ResultPattern<string>> ChangePasswordAsync(ChangePasswordRequest request) 
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null || user.IsDeleted)
            return ResultPattern<string>.Failure("Não autorizado!", "Não autorizado!", StatusCodes.Status401Unauthorized);

        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
            return ResultPattern<string>.Failure(
                String.Join(Environment.NewLine, result.Errors.Select(e => $"- code={e.Code}, Description: {e.Description};")), 
                "Algo deu errado ao tentar alterar a sua senha, tente novamente em alguns minutos");

        return ResultPattern<string>.Success("Password atualizado com sucesso!");
    }

    public async Task<ResultPattern<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return ResultPattern<string>.Success("Se o e-mail estiver cadastrado, você receberá as instruções para recuperar sua senha.");

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetUrl = QueryHelpers.AddQueryString(
            $"{_configuration["App:PublicFrontendUrl"]}/recuperar-senha",
            new Dictionary<string, string?>
            {
                ["email"] = user.Email,
                ["token"] = token
            });

        //await _emailService.SendAsync(
        //    user.Email!,
        //    "Recuperação de senha - OdontoClass",
        //    $"""
        //        <p>Recebemos uma solicitação para redefinir sua senha.</p>
        //        <p>
        //            <a href="{HtmlEncoder.Default.Encode(resetUrl)}">
        //                Redefinir minha senha
        //            </a>
        //        </p>
        //        <p>Se você não solicitou a recuperação, ignore este e-mail.</p>
        //    """);

        return ResultPattern<string>.Success("Se o e-mail estiver cadastrado, você receberá as instruções para recuperar sua senha.");
    }

    public async Task<ResultPattern<string>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return ResultPattern<string>.Failure("Não foi possível redefinir a senha.");

        var result = await _userManager.ResetPasswordAsync(
            user,
            request.Token,
            request.NewPassword);

        if (!result.Succeeded)
            return ResultPattern<string>.Failure(
            String.Join(Environment.NewLine, result.Errors.Select(e => $"- code={e.Code}, Description: {e.Description};")), "Algo deu errado ao tentar redefinir a sua senha tente novamente em instantes");

        return ResultPattern<string>.Success("Senha redefinida com sucesso.");
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
