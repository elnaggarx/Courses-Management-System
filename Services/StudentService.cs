using Microsoft.EntityFrameworkCore;
using WebApplication3.Database;
using WebApplication3.DTOs.Response;
using WebApplication3.Entities;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Select(s => new StudentResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                EnrolledCourses = s.Enrollments
                    .Select(e => new CourseSummaryDto
                    {
                        Id = e.CourseId,
                        Title = e.Course.Title
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<StudentResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Students
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new StudentResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                EnrolledCourses = s.Enrollments
                    .Select(e => new CourseSummaryDto
                    {
                        Id = e.CourseId,
                        Title = e.Course.Title
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StudentResponseDto> CreateAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return new StudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            EnrolledCourses = new List<CourseSummaryDto>()
        };
    }

    public async Task<bool> UpdateAsync(int id, Student student)
    {
        var existing = await _context.Students.FindAsync(id);
        if (existing is null) return false;

        existing.Name = student.Name;
        existing.Email = student.Email;

        _context.Students.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Students.FindAsync(id);
        if (existing is null) return false;

        _context.Students.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EnrollAsync(int studentId, int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student is null) return false;

        var course = await _context.Courses.FindAsync(courseId);
        if (course is null) return false;

        var existing = await _context.Enrollments.FindAsync(studentId, courseId);
        if (existing is not null) return true;

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            Student = student,
            Course = course
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeEnrollAsync(int studentId, int courseId)
    {
        var enrollment = await _context.Enrollments.FindAsync(studentId, courseId);
        if (enrollment is null) return false;

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CourseSummaryDto>> GetEnrolledCoursesAsync(int studentId)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course)
            .Select(e => new CourseSummaryDto
            {
                Id = e.Course!.Id,
                Title = e.Course.Title
            })
            .ToListAsync();
    }
}
