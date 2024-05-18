using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService userService):ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetUserDto>>> GetUsersAsync([FromQuery]UserFilter userFilter)
        => await userService.GetUserAsync(userFilter);

    [HttpGet("{userId:int}")]
    public async Task<Response<GetUserDto>> GetUserByIdAsync(int userId)
        => await userService.GetUserByIdAsync(userId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateUserAsync([FromBody]CreateUserDto user)
        => await userService.AddUserAsync(user);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateUserAsync([FromBody]UpdateUserDto user)
        => await userService.UpdateUserAsync(user);

    [HttpDelete("{userId:int}")]
    public async Task<Response<bool>> DeleteUserAsync(int userId)
        => await userService.DeleteUserAsync(userId);
}