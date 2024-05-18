using Domain.DTOs.AuthorBookDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.AuthorBookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthorBookController(IAuthorBookService authorBookService) : ControllerBase
{
    [HttpGet("get")]
    public async Task<Response<List<GetAuthorBookDto>>> GetAuthorBooksAsync([FromQuery]PaginationFilter filter)
        => await authorBookService.GetAuthorBooksAsync(filter);


    [HttpGet("{authorBookId:int}")]
    public async Task<Response<GetAuthorBookDto>> GetAuthorBookByIdAsync(int authorBookId)
        => await authorBookService.GetAuthorBooksByIdAsync(authorBookId);


    [HttpPost("create")]
    public async Task<Response<string>> CreateAuthorBookAsync([FromBody]CreateAuthorBookDto authorBook) =>
        await authorBookService.AddAuthorBookAsync(authorBook);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateAuthorBookAsync([FromBody]UpdateAuthorBookDto authorBook)
        => await authorBookService.UpdateAuthorBookAsync(authorBook);


    [HttpDelete("{authorBookId:int}")]
    public async Task<Response<bool>> DeleteAuthorBookAsync(int authorBookId)
        => await authorBookService.DeleteAuthorBookAsync(authorBookId);

}