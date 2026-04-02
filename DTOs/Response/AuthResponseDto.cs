namespace WebApplication3.DTOs.Response;

public class AuthResponseDto
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
