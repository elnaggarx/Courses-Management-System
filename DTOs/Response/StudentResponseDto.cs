using System.Collections.Generic;

namespace WebApplication3.DTOs.Response;

public class StudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<CourseSummaryDto> EnrolledCourses { get; set; } = new();
}
