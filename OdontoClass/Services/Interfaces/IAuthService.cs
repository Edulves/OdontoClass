using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests;
using OdontoClass.DTOs.Responses;

namespace OdontoClass.Services.Interfaces;

public interface IAuthService
{
    Task<ResultPattern<string>> RegisterAsync(RegisterRequest request);
    Task<ResultPattern<string>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<ResultPattern<TokenrResponse>> LoginAsync(LoginRequest request);
    Task<ResultPattern<MeResponse>> MeAsync(string userId);
    Task<ResultPattern<string>> ChangePasswordAsync(ChangePasswordRequest request);
}

