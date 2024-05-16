using Infrastructure.AutoMapper;
using WebApp.ExtensionMethods.AuthConfigurations;
using WebApp.ExtensionMethods.RegisterService;
using WebApp.ExtensionMethods.SwaggerConfigurations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// connection to database && dependency injection
builder.Services.AddRegisterService(builder.Configuration);

// register swagger configuration
builder.Services.SwaggerService();

// authentications service
builder.Services.AddAuthConfigureService(builder.Configuration);

// automapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();


