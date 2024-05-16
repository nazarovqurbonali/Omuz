namespace Domain.Entities;

public class Book:BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int AgeLimit { get; set; }
    public DateTimeOffset PublishedYear { get; set; }
    public int TotalPages { get; set; }
}