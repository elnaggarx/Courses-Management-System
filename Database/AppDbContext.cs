using Microsoft.EntityFrameworkCore;
using WebApplication3.Entities;
namespace WebApplication3.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
     public DbSet<Student> Students { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Instructor> Instructors { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<InstructorProfile> InstructorProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Instructor>()
            .HasOne(i => i.InstructorProfile)
            .WithOne(p => p.Instructor)
            .HasForeignKey<InstructorProfile>(p => p.InstructorId);

        // Seed data for initial sample
        modelBuilder.Entity<Instructor>().HasData(
            new Instructor { Id = 1, Name = "Alice Johnson", Email = "alice@localhost" },
            new Instructor { Id = 2, Name = "Bob Smith", Email = "bob@localhost" }
        );

        modelBuilder.Entity<InstructorProfile>().HasData(
            new InstructorProfile { Id = 1, InstructorId = 1, Bio = "Expert in Mathematics and Data Science." },
            new InstructorProfile { Id = 2, InstructorId = 2, Bio = "Passionate about Web Development and Cloud." }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Title = "Intro to C#", InstructorId = 2 },
            new Course { Id = 2, Title = "Data Structures", InstructorId = 1 },
            new Course { Id = 3, Title = "Entity Framework Core", InstructorId = 2 }
        );

        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "Charlie Brown", Email = "charlie@student.local" },
            new Student { Id = 2, Name = "Dana White", Email = "dana@student.local" },
            new Student { Id = 3, Name = "Evan Davis", Email = "evan@student.local" }
        );

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { StudentId = 1, CourseId = 1 },
            new Enrollment { StudentId = 1, CourseId = 2 },
            new Enrollment { StudentId = 2, CourseId = 2 },
            new Enrollment { StudentId = 3, CourseId = 3 }
        );
    }
}