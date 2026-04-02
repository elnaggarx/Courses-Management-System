using WebApplication3.DTOs.Response;
using WebApplication3.Entities;

namespace WebApplication3.Interfaces;

public interface IInstructorService
{
    Task<IEnumerable<InstructorResponseDto>> GetAllAsync();
    Task<InstructorResponseDto?> GetByIdAsync(int id);
    Task<InstructorResponseDto> CreateAsync(Instructor instructor);
    Task<bool> UpdateAsync(int id, Instructor instructor);
    Task<bool> DeleteAsync(int id);
    Task<InstructorProfile?> GetProfileAsync(int instructorId);
    Task<bool> UpdateProfileAsync(int instructorId, InstructorProfile profile);
}