using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests;

public sealed record LoginRequest(
    [Required, EmailAddress]
string Email,

    [Required]
string Password);
