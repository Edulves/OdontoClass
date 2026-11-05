using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests.Auth;
using OdontoClass.DTOs.Responses.Auth;

namespace OdontoClass.Services.Interfaces;

public interface IAuthService
{
    Task<ResultPattern<string>> RegisterAsync(RegisterRequest request);
    Task<ResultPattern<string>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<ResultPattern<TokenrResponse>> LoginAsync(LoginRequest request);
    Task<ResultPattern<MeResponse>> MeAsync();
    Task<ResultPattern<string>> ChangePasswordAsync(ChangePasswordRequest request);
    Task<ResultPattern<string>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<ResultPattern<string>> ResetPasswordAsync(ResetPasswordRequest request);
}

