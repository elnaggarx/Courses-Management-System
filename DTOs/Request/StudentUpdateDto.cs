using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Request;

public class StudentUpdateDto
{
    [Required, MinLength(2), MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;
}
