using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.Services.Interfaces;
using System.Security.Claims;

namespace OdontoClass.Services;

public sealed class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
{
    public ResultPattern<int> GetCurrentUser()
    {
        var UserId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

        if (UserId is null)
            return ResultPattern<int>.Failure("Usuario não está logado!");

        if (!int.TryParse(UserId, out int intId))
            return ResultPattern<int>.Failure("ID do usuário inválido.");

        return ResultPattern<int>.Success(intId);
    }
}
