using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests.Auth;

public sealed record ChangePasswordRequest(
    [Required]
    string CurrentPassword,

    [Required, MinLength(8)]
    string NewPassword,
    
    [Required]
    string UserId);