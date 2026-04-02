using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Request;
using WebApplication3.DTOs.Response;
using WebApplication3.Entities;
using WebApplication3.Interfaces;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var instructors = await _instructorService.GetAllAsync();
        return Ok(instructors);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var instructor = await _instructorService.GetByIdAsync(id);
        if (instructor is null) return NotFound();
        return Ok(instructor);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] InstructorCreateDto dto)
    {
        var instructor = new Instructor
        {
            Name = dto.Name,
            Email = dto.Email
        };

        if (dto.Bio is not null)
        {
            instructor.InstructorProfile = new InstructorProfile { Bio = dto.Bio };
        }

        var created = await _instructorService.CreateAsync(instructor);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] InstructorUpdateDto dto)
    {
        var instructor = new Instructor
        {
            Name = dto.Name,
            Email = dto.Email
        };

        var updated = await _instructorService.UpdateAsync(id, instructor);
        if (!updated) return NotFound();

        if (dto.Bio is not null)
        {
            await _instructorService.UpdateProfileAsync(id, new InstructorProfile { Bio = dto.Bio });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _instructorService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id}/profile")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var profile = await _instructorService.GetProfileAsync(id);
        if (profile is null) return NotFound();

        return Ok(new InstructorProfileResponseDto { Bio = profile.Bio });
    }

    [HttpPut("{id}/profile")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] InstructorProfileDto dto)
    {
        var updated = await _instructorService.UpdateProfileAsync(id, new InstructorProfile { Bio = dto.Bio });
        return updated ? NoContent() : NotFound();
    }
}