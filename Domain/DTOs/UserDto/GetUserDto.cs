namespace Domain.DTOs.UserDto;

public class GetUserDto
{
    public  int Id { get; set; }
    public required string FullName { get; set; } 
    public required string Email { get; set; } 
    public required string HashPassword { get; set; }
    public required string PhoneNumber { get; set; }
}
