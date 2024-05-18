using Domain.DTOs.AuthorDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.AuthorService;

public interface IAuthorService
{
    Task<Response<List<GetAuthorDto>>> GetAuthorsAsync(AuthorFilter filter);
    Task<Response<GetAuthorDto>> GetAuthorsByIdAsync(int id);
    Task<Response<string>> AddAuthorAsync(CreateAuthorDto author);
    Task<Response<string>> UpdateAuthorAsync(UpdateAuthorDto author);
    Task<Response<bool>> DeleteAuthorAsync(int id);
}
