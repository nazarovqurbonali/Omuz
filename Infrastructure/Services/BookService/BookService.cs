using System.Net;
using Domain.DTOs.BookDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.BookService;

public class BookService(DataContext context, IFileService fileService) : IBookService
{
    #region GetBooksAsync

    public async Task<PagedResponse<List<GetBookDto>>> GetBooksAsync(BookFilter filter)
    {
        try
        {
            var books = context.AuthorBooks
                .Include(x => x.Book)
                .Include(x => x.Author).AsQueryable();
            
            if (!string.IsNullOrEmpty(filter.BookName))
                books = books.Where(x => x.Book!.Title.ToLower().Contains(filter.BookName.ToLower()));
            if (!string.IsNullOrEmpty(filter.AuthorName))
                books = books.Where(x =>
                    (x.Author!.FirstName! + x.Author.LastName).ToLower().Contains(filter.AuthorName.ToLower()));
            if (filter.Grade != null)
                books = books.Where(x => x.Book!.Grade == filter.Grade);
            if (filter.Subject != null)
                books = books.Where(x => x.Book!.Subject == filter.Subject);
            if (filter.BookCategory != null)
                books = books.Where(x => x.Book!.Category == filter.BookCategory);

            var response = await books.Select(x => new GetBookDto()
                {
                    Title = x.Book.Title,
                    PathBook = x.Book.PathBook,
                    Grade = x.Book.Grade,
                    Category = x.Book.Category,
                    Subject = x.Book.Subject,
                    Id = x.Book.Id,
                    Description = x.Book.Description,
                    AuthorName = x.Author.FirstName + " " + x.Author.LastName,
                    AgeLimit = x.Book.AgeLimit,
                    CreatedAt = x.Book.CreatedAt,
                    PublishedYear = x.Book.PublishedYear,
                    TotalPages = x.Book.TotalPages,
                    UpdatedAt = x.Book.UpdatedAt,
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord =await books.CountAsync();
            
            return new PagedResponse<List<GetBookDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetBookDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    public async Task<Response<GetBookDto>> GetBookByIdAsync(int bookId)
    {
        try
        {
            var existing = await context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if(existing==null) return new Response<GetBookDto>(HttpStatusCode.BadRequest,"Not Found");
        }
        catch (Exception e)
        {
            return new Response<GetBookDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> CreateBookAsync(CreateBookDto createBook)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> UpdateBookAsync(UpdateBookDto updateBook)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<bool>> DeleteBookAsync(int bookId)
    {
        throw new NotImplementedException();
    }
}