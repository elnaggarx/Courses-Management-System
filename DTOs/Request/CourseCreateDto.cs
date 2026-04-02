using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Request;

public class CourseCreateDto
{
    [Required, MinLength(2), MaxLength(150)]
    public string Title { get; set; }= string.Empty;

    [Required, Range(1, int.MaxValue)]
    public int InstructorId { get; set; }
}
