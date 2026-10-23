using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests;

public sealed record ForgotPasswordRequest(
    [Required]
    [EmailAddress]
    string Email
);