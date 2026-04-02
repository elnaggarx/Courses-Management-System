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
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course is null) return NotFound();
        return Ok(course);
    }

    [HttpPost]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            InstructorId = dto.InstructorId
        };

        var created = await _courseService.CreateAsync(course);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            InstructorId = dto.InstructorId
        };

        var updated = await _courseService.UpdateAsync(id, course);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _courseService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{courseId}/students")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> GetStudentsByCourse(int courseId)
    {
        var students = await _courseService.GetStudentsByCourseAsync(courseId);
        return Ok(students);
    }

    [HttpGet("{courseId}/instructor")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> GetCourseInstructor(int courseId)
    {
        var instructor = await _courseService.GetCourseInstructorAsync(courseId);
        if (instructor is null) return NotFound();
        return Ok(instructor);
    }
}
