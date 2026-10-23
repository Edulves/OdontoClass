using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests;

public record ResetPasswordRequest(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Token,

    [Required]
    [MinLength(8)]
    string NewPassword
);
