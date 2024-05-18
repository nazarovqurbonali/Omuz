using Domain.DTOs.BookDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookController(IBookService bookService):ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<Response<List<GetBookDto>>> GetBooksAsync([FromQuery]BookFilter bookFilter)
        => await bookService.GetBooksAsync(bookFilter);

    [HttpGet("{bookId:int}")]
    [AllowAnonymous]
    public async Task<Response<GetBookDto>> GetBookByIdAsync(int bookId)
        => await bookService.GetBookByIdAsync(bookId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateBookAsync([FromForm]CreateBookDto book)
        => await bookService.CreateBookAsync(book);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateBookAsync([FromForm]UpdateBookDto book)
        => await bookService.UpdateBookAsync(book);

    [HttpDelete("{bookId:int}")]
    public async Task<Response<bool>> DeleteBookAsync(int bookId)
        => await bookService.DeleteBookAsync(bookId);
}