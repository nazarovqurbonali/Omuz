using AutoMapper;
using Domain;
using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserService;

public class UserService(DataContext context) : IUserService
{
    
    
    public async Task<Response<string>> AddUserAsync(CreateUserDto addUserDto)
    {
        try
        {
            var mapped = _mapper.Map<User>(addUserDto);
            await _context.Users.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("User added successfully");
        }
        catch (Exception ex)
        {
            return new Response<string>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetUserDto>>> GetUserAsync(UserFilter filter)
    {
        try
        {
            var Users = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter.FullName))
                Users = Users.Where(x => x.FullName.ToLower().Contains(filter.FullName.ToLower()));

            var User = await Users.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await Users.CountAsync();

            var response = _mapper.Map<List<GetUserDto>>(Users);
            return new PagedResponse<List<GetUserDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserDto>>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var existing = await _context.Users.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0) return new Response<bool>(System.Net.HttpStatusCode.BadRequest, "User not found!");
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            return new Response<bool>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
        }
    }


    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            var existing = await _context.Users.AnyAsync(e => e.Id == updateUserDto.Id);
            if (!existing) return new Response<string>(System.Net.HttpStatusCode.BadRequest, "User not found!");
            var mapped = _mapper.Map<User>(updateUserDto);
            _context.Users.Update(mapped);

            await _context.SaveChangesAsync();
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            return new Response<string>(System.Net.HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<GetUserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return new Response<GetUserDto>(System.Net.HttpStatusCode.BadRequest, "User not found");
            var User = _mapper.Map<GetUserDto>(existing);
            return new Response<GetUserDto>(User);
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }
    }
}





