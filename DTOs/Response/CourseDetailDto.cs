using System.Collections.Generic;

namespace WebApplication3.DTOs.Response;

public class CourseDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public InstructorSummaryDto Instructor { get; set; }
    public List<StudentResponseDto> Students { get; set; } = new();
}