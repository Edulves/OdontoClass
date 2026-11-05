namespace OdontoClass.Models;

public class WorkSpace
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public ICollection<WorkSpaceUser> WorkSpaceMembers { get; set; } = [];
    public ICollection<Schedule> Schedules { get; set; } = [];
}