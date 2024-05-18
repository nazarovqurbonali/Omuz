using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.BookDto;

public class GetBookDto
{
    public int Id { get; set; }
    public required string Title { get; set; } 
    public string? Description { get; set; }
    public string? AuthorName { get; set; }
    public int AgeLimit { get; set; }
    public DateTimeOffset PublishedYear { get; set; }
    public int TotalPages { get; set; }
    public required  string PathBook  { get; set; }
    public BookCategory Category { get; set; }
    public Grade? Grade { get; set; }
    public Subject? Subject { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public  DateTimeOffset UpdatedAt { get; set; }
}
