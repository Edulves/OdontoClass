using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests;

public sealed record ChangePasswordRequest(
    [Required]
    string CurrentPassword,

    [Required, MinLength(8)]
    string NewPassword);