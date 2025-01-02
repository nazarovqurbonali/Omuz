using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using WebApp.ExtensionMethods.AuthConfigurations;
using WebApp.ExtensionMethods.RegisterService;
using WebApp.ExtensionMethods.SwaggerConfigurations;

WebAppplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// connection to database && dependency injection
builder.Services.AddRegisterService(builder.Configuration);


// register swagger configuration
builder.Services.SwaggerService();

// authentications service
builder.Services.AddAuthConfigureService(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseCors(
    build => build.WithOrigins( "http://localhost:3000","https://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
);

try {
    var serviceProvider = app.Services.CreateScope().ServiceProvider; 
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    
    // seed data
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedUser();
}
catch (Exception)
{
    // ignored
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
