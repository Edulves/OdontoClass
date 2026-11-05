using System.ComponentModel.DataAnnotations;

namespace OdontoClass.DTOs.Requests.Schedule;

public sealed record PostWorkSpaceRequest([Required] string WorkSpaceName);
