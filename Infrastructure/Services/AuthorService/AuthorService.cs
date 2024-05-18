using Domain.DTOs.AuthorDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.AuthorService;

public class AuthorService(DataContext context):IAuthorService
{

    #region GetAuthorsAsync

    public async Task<Response<List<GetAuthorDto>>> GetAuthorsAsync(AuthorFilter filter)
    {
        try
        {
            var query = context.Authors.AsQueryable();
            
            if (!string.IsNullOrEmpty(filter.FirstName))
                query = query.Where(s =>
                    s.FirstName!.ToLower().Contains(filter.FirstName.ToLower()));

            var totalRecord = await query.CountAsync();

            var authors = await query.Select(x => new GetAuthorDto()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DoB = x.DoB,
                    IsAlive = x.IsAlive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                    
                }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();

            return new PagedResponse<List<GetAuthorDto>>(authors, totalRecord, filter.PageNumber,filter.PageSize);
        }
        catch (Exception e)
        {
            return new Response<List<GetAuthorDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetAuthorByIdAsync

    public async Task<Response<GetAuthorDto>> GetAuthorByIdAsync(int id)
    {
        try
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if(author==null) return new Response<GetAuthorDto>(HttpStatusCode.NotFound,"Author not found");
            var result = new GetAuthorDto()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DoB = author.DoB,
                IsAlive = author.IsAlive,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt    
            };

            return new Response<GetAuthorDto>(result);
        }
        catch (Exception e)
        {
            return new Response<GetAuthorDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
    

    #endregion

    #region AddAuthorAsync

    public async Task<Response<string>> AddAuthorAsync(CreateAuthorDto model)
    {
        try
        {
            var newAuthor = new Author()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DoB = model.DoB,
                IsAlive = model.IsAlive,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await context.Authors.AddAsync(newAuthor);
            await context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "Successfully added Author");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateAuthorAsync

    public async Task<Response<string>> UpdateAuthorAsync(UpdateAuthorDto author)
    {
        try
        {
            var request = await context.Authors.FirstOrDefaultAsync(x=>x.Id==author.Id);
            if(request==null) return new Response<string>(HttpStatusCode.NotFound,"Author not found");

            request.Id = author.Id;
            request.FirstName = author.FirstName;
            request.LastName = author.LastName;
            request.DoB = author.DoB;
            request.IsAlive = author.IsAlive;
            request.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "Author updated successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteAuthorAsync

    public async Task<Response<bool>> DeleteAuthorAsync(int id)
    {
        try
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)  return new Response<bool>(HttpStatusCode.NotFound,"Author not found");
            
            context.Authors.Remove(author);
            await context.SaveChangesAsync();
            
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}
