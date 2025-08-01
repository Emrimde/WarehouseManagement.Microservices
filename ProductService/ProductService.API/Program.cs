using ProductService.Core;
using ProductService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load();

// Add services to the container.
builder.Services.AddControllers(); // dodanie kontrolerów do kontenera Dependency Injection 
builder.Services.AddEndpointsApiExplorer(); // dodanie eksploratora punktów końcowych
builder.Services.AddSwaggerGen(); // dodanie Swaggera do kontenera Dependency Injection

builder.Services.AddInfrastructure();
builder.Services.AddCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Sprawdzenie, czy środowisko to środowisko deweloperskie
{
    app.UseSwagger(); // Włączenie Swaggera w środowisku deweloperskim
    app.UseSwaggerUI(); // Włączenie interfejsu użytkownika Swaggera w środowisku deweloperskim
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
