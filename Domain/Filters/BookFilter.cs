using Domain.Enums;

namespace Domain.Filters;

public class BookFilter:PaginationFilter
{
    public string? BookName { get; set; }
    public string? Description { get; set; }
    public Grade? Grade { get; set; }
    public Subject? Subject { get; set; }
    public BookCategory? BookCategory  { get; set; }
}