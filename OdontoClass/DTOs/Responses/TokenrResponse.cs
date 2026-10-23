namespace OdontoClass.DTOs.Responses;

public sealed record TokenrResponse(string AccessToken, string TokenType, int ExpiresIn);