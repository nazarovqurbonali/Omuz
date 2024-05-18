namespace Domain.Filters;

public class PaginationFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = 3;
    }
    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize < 3 ? 3: pageSize;
    }
}