using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests;

public sealed record RegisterRequest(
    [Required, EmailAddress]
    string Email,

    [Required, MinLength(8)]
    string Password);