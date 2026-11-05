using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests.Schedule;

public sealed record PostSchedulesRequest(
    [Required] DateTime DateOfSchedule,
    [Required] string Subject, 
    [Required] string Procedure, 
    [Required] string PacienteName, 
    [Required] int WorkSpaceId,
    string PacientInfo = "");