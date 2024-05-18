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
    public async Task<Response<List<GetAuthorDto>>> GetAuthorsAsync(AuthorFilter authorFilter)
        => await authorService.GetAuthorsAsync(authorFilter);

    [HttpGet("{AuthorId:int}")]
    public async Task<Response<GetAuthorDto>> GetAuthorByIdAsync(int authorId)
        => await authorService.GetAuthorsByIdAsync(authorId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateAuthorAsync(CreateAuthorDto author)
        => await authorService.AddAuthorAsync(author);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateAuthorAsync(UpdateAuthorDto author)
        => await authorService.UpdateAuthorAsync(author);

    [HttpDelete("{AuthorId:int}")]
    public async Task<Response<bool>> DeleteAuthorAsync(int AuthorId)
        => await authorService.DeleteAuthorAsync(AuthorId);
}