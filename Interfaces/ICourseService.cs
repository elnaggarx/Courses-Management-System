using WebApplication3.DTOs.Response;
using WebApplication3.Entities;

namespace WebApplication3.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllAsync();
    Task<CourseResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<StudentResponseDto>> GetStudentsByCourseAsync(int courseId);
    Task<InstructorSummaryDto?> GetCourseInstructorAsync(int courseId);
    Task<CourseResponseDto> CreateAsync(Course course);
    Task<bool> UpdateAsync(int id, Course course);
    Task<bool> DeleteAsync(int id);
}
