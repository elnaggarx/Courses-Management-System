using Microsoft.EntityFrameworkCore;
using WebApplication3.Database;
using WebApplication3.DTOs.Response;
using WebApplication3.Entities;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _context;

    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                InstructorId = c.InstructorId
            })
            .ToListAsync();
    }

    public async Task<CourseResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                InstructorId = c.InstructorId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<StudentResponseDto>> GetStudentsByCourseAsync(int courseId)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Student)
            .ThenInclude(s => s!.Enrollments)
            .ThenInclude(ec => ec.Course)
            .Select(e => new StudentResponseDto
            {
                Id = e.Student!.Id,
                Name = e.Student.Name,
                Email = e.Student.Email,
                EnrolledCourses = e.Student.Enrollments
                    .Select(ec => new CourseSummaryDto
                    {
                        Id = ec.CourseId,
                        Title = ec.Course!.Title
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<InstructorSummaryDto?> GetCourseInstructorAsync(int courseId)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => c.Instructor)
            .Select(i => new InstructorSummaryDto
            {
                Id = i!.Id,
                Name = i.Name,
                Email = i.Email,
                Bio = i.InstructorProfile!.Bio
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CourseResponseDto> CreateAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return new CourseResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            InstructorId = course.InstructorId
        };
    }

    public async Task<bool> UpdateAsync(int id, Course course)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing is null) return false;

        existing.Title = course.Title;
        existing.InstructorId = course.InstructorId;

        _context.Courses.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing is null) return false;

        _context.Courses.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
