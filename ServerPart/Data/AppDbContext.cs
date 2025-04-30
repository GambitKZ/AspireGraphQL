using Microsoft.EntityFrameworkCore;
using ServerPart.Models;

namespace ServerPart.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<CoursePlan> CoursePlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoursePlan>()
            .HasOne(cp => cp.Teacher)
            .WithMany()
            .HasForeignKey(cp => cp.TeacherId);

        modelBuilder.Entity<CoursePlan>()
            .HasOne(cp => cp.Course)
            .WithMany()
            .HasForeignKey(cp => cp.CourseId);

        modelBuilder.Entity<CoursePlan>()
            .HasMany(cp => cp.Students)
            .WithOne(s => s.CoursePlan)
            .HasForeignKey(s => s.CoursePlanId);

        modelBuilder.Entity<Student>()
            .Property(s => s.Gender)
            .HasConversion<string>();

        modelBuilder.Entity<Teacher>()
            .Property(s => s.Gender)
            .HasConversion<string>();
    }
}
