namespace Domain.DTOs.AuthorDto;

public class UpdateAuthorDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTimeOffset DoB { get; set; }
    public bool IsAlive { get; set; } = true;
}
