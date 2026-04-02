using WebApplication3.DTOs.Response;
using WebApplication3.Entities;

namespace WebApplication3.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<CourseSummaryDto>> GetEnrolledCoursesAsync(int studentId);
    Task<StudentResponseDto> CreateAsync(Student student);
    Task<bool> UpdateAsync(int id, Student student);
    Task<bool> DeleteAsync(int id);
    Task<bool> EnrollAsync(int studentId, int courseId);
    Task<bool> DeEnrollAsync(int studentId, int courseId);
}
