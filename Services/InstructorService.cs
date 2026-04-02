using Microsoft.EntityFrameworkCore;
using WebApplication3.Database;
using WebApplication3.DTOs.Response;
using WebApplication3.Entities;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class InstructorService : IInstructorService
{
    private readonly AppDbContext _context;

    public InstructorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InstructorResponseDto>> GetAllAsync()
    {
        return await _context.Instructors
            .AsNoTracking()
            .Select(i => new InstructorResponseDto
            {
                Id = i.Id,
                Name = i.Name,
                Email = i.Email,
                Profile = i.InstructorProfile == null ? null : new InstructorProfileResponseDto { Bio = i.InstructorProfile.Bio },
                Courses = i.Courses.Select(c => new CourseSummaryDto { Id = c.Id, Title = c.Title }).ToList()
            })
            .ToListAsync();
    }

    public async Task<InstructorResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Instructors
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new InstructorResponseDto
            {
                Id = i.Id,
                Name = i.Name,
                Email = i.Email,
                Profile = i.InstructorProfile == null ? null : new InstructorProfileResponseDto { Bio = i.InstructorProfile.Bio },
                Courses = i.Courses.Select(c => new CourseSummaryDto { Id = c.Id, Title = c.Title }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<InstructorResponseDto> CreateAsync(Instructor instructor)
    {
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        return new InstructorResponseDto
        {
            Id = instructor.Id,
            Name = instructor.Name,
            Email = instructor.Email,
            Profile = instructor.InstructorProfile is null ? null : new InstructorProfileResponseDto { Bio = instructor.InstructorProfile.Bio },
            Courses = new List<CourseSummaryDto>()
        };
    }

    public async Task<bool> UpdateAsync(int id, Instructor instructor)
    {
        var existing = await _context.Instructors.FindAsync(id);
        if (existing is null) return false;

        existing.Name = instructor.Name;
        existing.Email = instructor.Email;

        _context.Instructors.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Instructors.FindAsync(id);
        if (existing is null) return false;

        _context.Instructors.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<InstructorProfile?> GetProfileAsync(int instructorId)
    {
        return await _context.InstructorProfiles
            .Include(p => p.Instructor)
            .FirstOrDefaultAsync(p => p.InstructorId == instructorId);
    }

    public async Task<bool> UpdateProfileAsync(int instructorId, InstructorProfile profile)
    {
        var existing = await _context.InstructorProfiles
            .FirstOrDefaultAsync(p => p.InstructorId == instructorId);

        if (existing is null) return false;

        existing.Bio = profile.Bio;
        _context.InstructorProfiles.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}