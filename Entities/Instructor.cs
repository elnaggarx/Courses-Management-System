using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Entities;

public class Instructor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public InstructorProfile? InstructorProfile { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}