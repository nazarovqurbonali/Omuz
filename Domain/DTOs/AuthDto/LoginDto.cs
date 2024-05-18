namespace Domain.DTOs.AuthDto;

public class LoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}