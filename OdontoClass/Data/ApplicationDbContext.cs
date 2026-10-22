using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdontoClass.Models;

namespace OdontoClass.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<Schedule> Schedules => Set<Schedule>();

    public DbSet<ScheduleUser> ScheduleUsers => Set<ScheduleUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("asp_net_users");
        builder.Entity<IdentityRole<int>>().ToTable("asp_net_roles");

        builder.Entity<IdentityUserRole<int>>()
            .ToTable("asp_net_user_roles");

        builder.Entity<IdentityUserClaim<int>>()
            .ToTable("asp_net_user_claims");

        builder.Entity<IdentityUserLogin<int>>()
            .ToTable("asp_net_user_logins");

        builder.Entity<IdentityRoleClaim<int>>()
            .ToTable("asp_net_role_claims");

        builder.Entity<IdentityUserToken<int>>()
            .ToTable("asp_net_user_tokens");

        builder.Entity<Schedule>(entity =>
        {
            entity.HasKey(s => s.ScheduleId);

            entity.Property(s => s.PacienteName)
                .IsRequired();

            entity.Property(s => s.Procedure)
                .IsRequired();

            entity.Property(s => s.PacienteInfo)
                .IsRequired(false);

            entity.Property(s => s.Subject)
                .IsRequired(false);
        });

        builder.Entity<ScheduleUser>(entity =>
        {
            entity.HasKey(su => new
            {
                su.UserId,
                su.ScheduleId
            });

            entity.HasOne(su => su.User)
                .WithMany(su => su.ScheduleUsers)
                .HasForeignKey(su => su.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(su => su.Schedule)
                .WithMany(s => s.ScheduleUsers)
                .HasForeignKey(su => su.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
