using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IAuthService, AuthService>();

    }
}