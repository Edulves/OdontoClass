namespace OdontoClass.DTOs.Responses.Auth;

public sealed record MeResponse(int Id, string Email, string UserName, DateTime CreatedAt);
