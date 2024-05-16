namespace Domain.Entities;

public class User:BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string HashPassword { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

}