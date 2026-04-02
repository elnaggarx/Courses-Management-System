using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Request;

public class CourseUpdateDto
{
    [Required, MinLength(2), MaxLength(150)]
    public string Title { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int InstructorId { get; set; }
}
