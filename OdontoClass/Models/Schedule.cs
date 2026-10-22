namespace OdontoClass.Models;

public class Schedule
{
    public int ScheduleId { get; set; }

    public DateOnly DateOfSchedule { get; set; }

    public string PacienteName { get; set; } = string.Empty;

    public string? PacienteInfo { get; set; }

    public string? Subject { get; set; }

    public string Procedure { get; set; } = string.Empty;

    public bool Deleted { get; set; }

    public ICollection<ScheduleUser> ScheduleUsers { get; set; } = [];
}
