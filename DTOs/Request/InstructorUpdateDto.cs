using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Request;

public class InstructorUpdateDto
{
    [Required, MinLength(2), MaxLength(100)]
    public string Name { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }
}