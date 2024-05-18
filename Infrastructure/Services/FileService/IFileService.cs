using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public interface IFileService
{
    Task<Response<string>> CreateFile(IFormFile file);
    Response<bool> DeleteFile(string file);
}