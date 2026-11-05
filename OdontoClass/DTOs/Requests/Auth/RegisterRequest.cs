using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests.Auth;

public sealed record RegisterRequest(
    [Required, EmailAddress]
    string Email,

    [Required, MinLength(8)]
    string Password);