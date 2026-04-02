using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Request;

public class InstructorProfileDto
{
    [Required, MaxLength(1000)]
    public string Bio { get; set; }
}