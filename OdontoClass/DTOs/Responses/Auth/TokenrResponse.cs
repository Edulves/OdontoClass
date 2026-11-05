namespace OdontoClass.DTOs.Responses.Auth;

public sealed record TokenrResponse(string AccessToken, string TokenType, int ExpiresIn);