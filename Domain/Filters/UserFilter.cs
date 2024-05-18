namespace Domain.Filters;

public class UserFilter:PaginationFilter
{
    public string? UserName { get; set; }
}