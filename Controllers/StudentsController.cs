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
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student is null) return NotFound();
        return Ok(student);
    }

    [HttpPost]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
    {
        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email
        };

        var created = await _studentService.CreateAsync(student);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
    {
        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email
        };

        var updated = await _studentService.UpdateAsync(id, student);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{studentId}/enroll/{courseId}")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> Enroll(int studentId, int courseId)
    {
        var result = await _studentService.EnrollAsync(studentId, courseId);
        return result ? Ok(new { studentId, courseId, status = "enrolled" }) : NotFound();
    }

    [HttpPost("{studentId}/deenroll/{courseId}")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> DeEnroll(int studentId, int courseId)
    {
        var result = await _studentService.DeEnrollAsync(studentId, courseId);
        return result ? Ok(new { studentId, courseId, status = "deenrolled" }) : NotFound();
    }

    [HttpGet("{studentId}/courses")]
    [Authorize(Policy = "UserOrHigher")]
    public async Task<IActionResult> GetEnrolledCourses(int studentId)
    {
        var courses = await _studentService.GetEnrolledCoursesAsync(studentId);
        return Ok(courses);
    }
}
