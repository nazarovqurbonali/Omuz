using Domain.Enums;

namespace Domain.Entities;

public class Book:BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int AgeLimit { get; set; }
    public DateTimeOffset PublishedYear { get; set; }
    public int TotalPages { get; set; }
    public string PathBook  { get; set; }=null!;
    public BookCategory Category { get; set; }
    public Grade? Grade { get; set; }
    public Subject? Subject { get; set; }
    public List<AuthorBook>? AuthorBooks { get; set; }
}