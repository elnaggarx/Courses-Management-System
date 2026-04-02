using System.Collections.Generic;

namespace WebApplication3.DTOs.Response;

public class InstructorResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public InstructorProfileResponseDto? Profile { get; set; }
    public List<CourseSummaryDto> Courses { get; set; } = new();
}
