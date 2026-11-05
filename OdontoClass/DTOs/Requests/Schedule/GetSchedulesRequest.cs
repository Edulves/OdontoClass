using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests.Schedule;

public sealed record GetSchedulesRequest([Required] int Month, [Required] int Year, string PacientName, string Subject, [Required] int WorkSpaceId);
