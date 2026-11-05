using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdontoClass.Models;

namespace OdontoClass.Data.Context;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<Schedule> Schedule => Set<Schedule>();
    public DbSet<WorkSpace> WorkSpace => Set<WorkSpace>();
    public DbSet<WorkSpaceUser> WorkSpaceUser => Set<WorkSpaceUser>();

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

            entity.Property(s => s.PacientName)
                .IsRequired();

            entity.Property(s => s.Procedure)
                .IsRequired();

            entity.Property(s => s.PacientInfo)
                .IsRequired(false);

            entity.Property(s => s.Subject)
                .IsRequired(false);

            entity.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.WorkSpace)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.WorkSpaceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<WorkSpaceUser>(entity =>
        {
            entity.HasKey(x => new
            {
                x.WorkSpaceId,
                x.UserId
            });

            entity.HasOne(x => x.WorkSpace)
                .WithMany(x => x.WorkSpaceMembers)
                .HasForeignKey(x => x.WorkSpaceId);

            entity.HasOne(x => x.User)
                .WithMany(x => x.WorkSpaceMembers)
                .HasForeignKey(x => x.UserId);
        });

        builder.Entity<WorkSpace>()
            .HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
