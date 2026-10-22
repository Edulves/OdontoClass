using Microsoft.AspNetCore.Identity;

namespace OdontoClass.Models;

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public ICollection<ScheduleUser> ScheduleUsers { get; set; } = [];
}
