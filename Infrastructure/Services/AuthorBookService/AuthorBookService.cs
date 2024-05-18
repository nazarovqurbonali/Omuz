using Domain.DTOs.AuthorBookDto;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Domain.Filters;

namespace Infrastructure.Services.AuthorBookService;

public class AuthorBookService(DataContext context) : IAuthorBookService
{
    #region GetAuthorBooksAsync

    public async Task<Response<List<GetAuthorBookDto>>> GetAuthorBooksAsync(PaginationFilter filter)
    {
        try
        {
            var query = context.AuthorBooks.AsQueryable();


            var authorBooks = await query.Select(x => new GetAuthorBookDto()
            {
                Id = x.Id,
                AuthorId = x.AuthorId,
                BookId = x.BookId,
                CreateAt = x.CreatedAt,
                UpdateAt = x.UpdatedAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var totalRecord = await query.CountAsync();


            return new PagedResponse<List<GetAuthorBookDto>>(authorBooks, totalRecord, filter.PageNumber,
                filter.PageNumber);
        }
        catch (Exception e)
        {
            return new Response<List<GetAuthorBookDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetAuthorBooksByIdAsync

    public async Task<Response<GetAuthorBookDto>> GetAuthorBooksByIdAsync(int id)
    {
        try
        {
            var authorBook = await context.AuthorBooks.FirstOrDefaultAsync(x => x.Id == id);
            if (authorBook == null)
                return new Response<GetAuthorBookDto>(HttpStatusCode.NotFound, "AuthorBook not found");
            var result = new GetAuthorBookDto()
            {
                AuthorId = authorBook.AuthorId,
                BookId = authorBook.BookId,
                Id = authorBook.Id,
                CreateAt = authorBook.CreatedAt,
                UpdateAt = authorBook.UpdatedAt
            };

            return new Response<GetAuthorBookDto>(result);
        }
        catch (Exception e)
        {
            return new Response<GetAuthorBookDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
    

    #endregion

    #region AddAuthorBookAsync

    public async Task<Response<string>> AddAuthorBookAsync(CreateAuthorBookDto model)
    {
        try
        {
            var newAuthorBook = new AuthorBook()
            {
                AuthorId = model.AuthorId,
                BookId = model.BookId,
                UpdatedAt = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await context.AuthorBooks.AddAsync(newAuthorBook);
            await context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "Successfully added AuthorBook");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateAuthorBookAsync

    public async Task<Response<string>> UpdateAuthorBookAsync(UpdateAuthorBookDto authorBook)
    {
        try
        {
            var request = await context.AuthorBooks.FirstOrDefaultAsync(x => x.Id == authorBook.Id);
            if (request == null) return new Response<string>(HttpStatusCode.NotFound, "AuthorBook not found");

            request.Id = authorBook.Id;
            request.AuthorId = authorBook.Id;
            request.BookId = authorBook.BookId;
            request.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "AuthorBook updated successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

   
    #endregion

    #region DeleteAuthorBookAsync

    public async Task<Response<bool>> DeleteAuthorBookAsync(int id)
    {
        try
        {
            var book = await context.AuthorBooks.Where(x => x.Id == id).ExecuteDeleteAsync();
            if(book==0) return new Response<bool>(HttpStatusCode.BadRequest,"Book not found");
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

}