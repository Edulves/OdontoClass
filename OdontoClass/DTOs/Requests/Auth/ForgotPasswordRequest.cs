using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests.Auth;

public sealed record ForgotPasswordRequest(
    [Required]
    [EmailAddress]
    string Email
);