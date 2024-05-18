using System.Net;
using Domain.DTOs.BookDto;
using Domain.Entities;
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
            var books = context.Books.Include(x => x.AuthorBooks).AsQueryable();

            if (!string.IsNullOrEmpty(filter.BookName))
                books = books.Where(x => x.Title.ToLower().Contains(filter.BookName.ToLower()));

            if (!string.IsNullOrEmpty(filter.Description))
                books = books.Where(x => x.Description!.ToLower().Contains(filter.Description.ToLower()));


            if (filter.Grade != null)
                books = books.Where(x => x.Grade == filter.Grade);
            if (filter.Subject != null)
                books = books.Where(x => x.Subject == filter.Subject);
            if (filter.BookCategory != null)
                books = books.Where(x => x.Category == filter.BookCategory);

            var response = await books.Select(x => new GetBookDto()
                {
                    Title = x.Title,
                    PathBook = x.PathBook,
                    Grade = x.Grade,
                    Category = x.Category,
                    Subject = x.Subject,
                    Id = x.Id,
                    AuthorName = context.AuthorBooks.Where(a => a.BookId == x.Id)
                        .Select(t => t.Author!.FirstName + " " + t.Author.LastName).FirstOrDefault(),
                    Description = x.Description,
                    AgeLimit = x.AgeLimit,
                    CreatedAt = x.CreatedAt,
                    PublishedYear = x.PublishedYear,
                    TotalPages = x.TotalPages,
                    UpdatedAt = x.UpdatedAt,
                }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await books.CountAsync();

            return new PagedResponse<List<GetBookDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetBookDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetBookByIdAsync

    public async Task<Response<GetBookDto>> GetBookByIdAsync(int bookId)
    {
        try
        {
            var existing = await context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (existing == null) return new Response<GetBookDto>(HttpStatusCode.BadRequest, "Not Found");
            var response = new GetBookDto()
            {
                Title = existing.Title,
                PathBook = existing.PathBook,
                Grade = existing.Grade,
                Category = existing.Category,
                Subject = existing.Subject,
                Id = existing.Id,
                Description = existing.Description,
                AuthorName = await context.AuthorBooks.Where(x => x.BookId == bookId)
                    .Select(x => x.Author!.FirstName + " " + x.Author.LastName).FirstOrDefaultAsync(),
                CreatedAt = existing.CreatedAt,
                PublishedYear = existing.PublishedYear,
                TotalPages = existing.TotalPages,
                UpdatedAt = existing.UpdatedAt,
                AgeLimit = existing.AgeLimit
            };
            return new Response<GetBookDto>(response);
        }
        catch (Exception e)
        {
            return new Response<GetBookDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateBookAsync

    public async Task<Response<string>> CreateBookAsync(CreateBookDto createBook)
    {
        try
        {
            var book = new Book()
            {
                AgeLimit = createBook.AgeLimit,
                CreatedAt = DateTimeOffset.UtcNow,
                Description = createBook.Description,
                Category = createBook.Category,
                Subject = createBook.Subject,
                PublishedYear = createBook.PublishedYear,
                TotalPages = createBook.TotalPages,
                UpdatedAt = DateTimeOffset.UtcNow,
                Grade = createBook.Grade,
                PathBook = await fileService.CreateFile(createBook.PathBook),
                Title = createBook.Title,
            };
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully created book");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateBookAsync

    public async Task<Response<string>> UpdateBookAsync(UpdateBookDto updateBook)
    {
        try
        {
            var existingBook = await context.Books.FirstOrDefaultAsync(x => x.Id == updateBook.Id);
            if (existingBook == null) return new Response<string>(HttpStatusCode.BadRequest, "Book not found");

            if (updateBook.PathBook != null)
            {
                fileService.DeleteFile(existingBook.PathBook);
                existingBook.PathBook = await fileService.CreateFile(updateBook.PathBook);
            }

            existingBook.AgeLimit = updateBook.AgeLimit;
            existingBook.Category = updateBook.Category;
            existingBook.Description = updateBook.Description;
            existingBook.Grade = updateBook.Grade;
            existingBook.TotalPages = updateBook.TotalPages;
            existingBook.Subject = updateBook.Subject;
            existingBook.PublishedYear = updateBook.PublishedYear;
            existingBook.CreatedAt = existingBook.CreatedAt;
            existingBook.UpdatedAt = DateTimeOffset.UtcNow;
            existingBook.Title = updateBook.Title;
            existingBook.Id = updateBook.Id;

            await context.SaveChangesAsync();
            return new Response<string>("Successfully updated the book");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteBookAsync

    public async Task<Response<bool>> DeleteBookAsync(int bookId)
    {
        try
        {
            var existing = await context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            fileService.DeleteFile(existing.PathBook);
            context.Books.Remove(existing);
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