namespace OdontoClass.Models;

public class WorkSpaceUser
{
    public int WorkSpaceId { get; set; }
    public WorkSpace WorkSpace { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
