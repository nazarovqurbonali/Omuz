using Domain.DTOs.AuthorDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.AuthorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetAuthorDto>>> GetAuthorsAsync([FromQuery] AuthorFilter authorFilter)
        => await authorService.GetAuthorsAsync(authorFilter);

    [HttpGet("{authorId:int}")]
    public async Task<Response<GetAuthorDto>> GetAuthorByIdAsync( int authorId)
        => await authorService.GetAuthorByIdAsync(authorId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateAuthorAsync([FromBody] CreateAuthorDto author)
        => await authorService.AddAuthorAsync(author);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateAuthorAsync([FromBody] UpdateAuthorDto author)
        => await authorService.UpdateAuthorAsync(author);

    [HttpDelete("{authorId:int}")]
    public async Task<Response<bool>> DeleteAuthorAsync( int authorId)
        => await authorService.DeleteAuthorAsync(authorId);
}