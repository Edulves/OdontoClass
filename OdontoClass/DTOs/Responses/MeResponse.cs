namespace OdontoClass.DTOs.Responses;

public sealed record MeResponse(int Id, string Email, string UserName, DateTime CreatedAt);
