using Domain.DTOs.AuthorBookDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.AuthorBookService;

public interface IAuthorBookService
{
    Task<Response<List<GetAuthorBookDto>>> GetAuthorBooksAsync(PaginationFilter filter);
    Task<Response<GetAuthorBookDto>> GetAuthorBooksByIdAsync(int id);
    Task<Response<string>> AddAuthorBookAsync(CreateAuthorBookDto authorBook);
    Task<Response<string>> UpdateAuthorBookAsync(UpdateAuthorBookDto authorBook);
    Task<Response<bool>> DeleteAuthorBookAsync(int id);
}
