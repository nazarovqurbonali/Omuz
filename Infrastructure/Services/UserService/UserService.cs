using System.Net;
using System.Security.Cryptography;
using System.Text;
using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserService;

public class UserService(DataContext context) : IUserService
{
    #region AddUserAsync

    public async Task<Response<string>> AddUserAsync(CreateUserDto addUserDto)
    {
        try
        {
            var existing = await context.Users.AnyAsync(x => x.Email == addUserDto.Email);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "User already exists");

            var user = new User()
            {
                FullName = addUserDto.FullName,
                Email = addUserDto.Email,
                HashPassword = ConvertToHash(addUserDto.HashPassword) ,
                PhoneNumber = addUserDto.PhoneNumber,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return new Response<string>("User added successfully");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion

    #region GetUserAsync

    public async Task<PagedResponse<List<GetUserDto>>> GetUserAsync(UserFilter filter)
    {
        try
        {
            var query = context.Users.AsQueryable();


            if (!string.IsNullOrEmpty(filter.FullName))
                query = query.Where(x => x.FullName.ToLower().Contains(filter.FullName.ToLower()));

            var user = await query.Select(x => new GetUserDto()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    HashPassword = x.HashPassword,
                    PhoneNumber = x.PhoneNumber,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await query.CountAsync();
            return new PagedResponse<List<GetUserDto>>(user, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteUserAsync

    public async Task<Response<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return new Response<bool>(HttpStatusCode.NotFound, "User Not Found");

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return new Response<bool>(HttpStatusCode.OK, "User deleted succesfuly");
        }
        catch (Exception ex)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion
    
    #region UpdateUserAsync

    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            var request = await context.Users.FirstOrDefaultAsync(e => e.Id == updateUserDto.Id);
            if (request == null) return new Response<string>(HttpStatusCode.NotFound, "User Not Found");

            request.Id = updateUserDto.Id;
            request.FullName = updateUserDto.FullName;
            request.Email = updateUserDto.Email;
            request.HashPassword = ConvertToHash(updateUserDto.HashPassword);
            request.PhoneNumber = updateUserDto.PhoneNumber;
            request.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "User Updated Successfuly");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion

    #region GetUserByIdAsync

    public async Task<Response<GetUserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null) return new Response<GetUserDto>(HttpStatusCode.NotFound, "User Not Found");
            var result = new GetUserDto()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                HashPassword = user.HashPassword,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
                
            };
            return new Response<GetUserDto>(result);
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region ConvertToHash

    private static string ConvertToHash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes =  sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    #endregion
}