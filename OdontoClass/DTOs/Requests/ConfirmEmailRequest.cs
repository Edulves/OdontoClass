using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests;

public sealed record ConfirmEmailRequest(
    [Required]
    int UserId, 

    [Required]
    string Token);