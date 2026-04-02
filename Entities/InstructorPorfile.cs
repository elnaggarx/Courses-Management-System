using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Entities;

public class InstructorProfile
{
    public int Id { get; set; }

    public string Bio { get; set; } = string.Empty;

    public int InstructorId { get; set; }

    public Instructor? Instructor { get; set; }
}