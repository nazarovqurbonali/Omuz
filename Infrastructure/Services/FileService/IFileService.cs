using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public interface IFileService
{
    Task<string> CreateFile(IFormFile file);
    bool DeleteFile(string file);
}