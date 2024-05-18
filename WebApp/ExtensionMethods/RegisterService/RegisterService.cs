using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.AuthorBookService;
using Infrastructure.Services.AuthorService;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.BookService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IAuthorBookService, AuthorBookService>();

    }
}