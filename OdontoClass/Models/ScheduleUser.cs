namespace OdontoClass.Models;

public class ScheduleUser
{
    public int UserId { get; set; }
    public int ScheduleId { get; set; }
    public User User { get; set; } = null!;
    public Schedule Schedule { get; set; } = null!;
}
