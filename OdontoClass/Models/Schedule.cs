namespace OdontoClass.Models;

public class Schedule
{
    public int ScheduleId { get; set; }
    public DateTime DateOfSchedule { get; set; }
    public required string PacientName { get; set; }
    public string PacientInfo { get; set; } = string.Empty;
    public required string Subject { get; set; }
    public required string Procedure { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int WorkSpaceId { get; set; }
    public WorkSpace WorkSpace { get; set; } = null!;
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public bool Deleted { get; set; }
}
