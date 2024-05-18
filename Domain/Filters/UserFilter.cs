namespace Domain.Filters;

public class UserFilter:PaginationFilter
{
    public string? FullName { get; set; }
}