using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.BookDto;

public class UpdateBookDto
{
    public int Id { get; set; }
    public required string Title { get; set; } 
    public string? Description { get; set; }
    public int AgeLimit { get; set; }
    public DateTimeOffset PublishedYear { get; set; }
    public int TotalPages { get; set; }
    public required  IFormFile PathBook  { get; set; }
    public BookCategory Category { get; set; }
    public Grade? Grade { get; set; }
    public Subject? Subject { get; set; }
    
}
