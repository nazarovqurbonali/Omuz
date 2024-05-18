using Domain.DTOs.BookDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.BookService;

public interface IBookService
{
    Task<PagedResponse<List<GetBookDto>>> GetBooksAsync(BookFilter filter);
    Task<Response<GetBookDto>> GetBookByIdAsync(int bookId);
    Task<Response<string>> CreateBookAsync(CreateBookDto createBook);
    Task<Response<string>> UpdateBookAsync(UpdateBookDto updateBook);
    Task<Response<bool>> DeleteBookAsync(int bookId);

}