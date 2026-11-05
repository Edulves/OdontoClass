using OdontoClass.Data.ResultPattern.Base;

namespace OdontoClass.Services.Interfaces;

public interface ICurrentUserService
{
    ResultPattern<int> GetCurrentUser();
}