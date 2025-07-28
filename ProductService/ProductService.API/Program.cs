using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
string host = Environment.GetEnvironmentVariable("HOST")!;
string username = Environment.GetEnvironmentVariable("USERNAME")!;
string password = Environment.GetEnvironmentVariable("PASSWORD")!;
string database = Environment.GetEnvironmentVariable("DATABASE")!;

string connectionString = $"Host={host};Username={username};Password={password};Database={database}";

// Add services to the container.
builder.Services.AddControllers(); // dodanie kontroler�w do kontenera Dependency Injection 
builder.Services.AddEndpointsApiExplorer(); // dodanie eksploratora punkt�w ko�cowych
builder.Services.AddSwaggerGen(); // dodanie Swaggera do kontenera Dependency Injection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Sprawdzenie, czy �rodowisko to �rodowisko deweloperskie
{
    app.UseSwagger(); // W��czenie Swaggera w �rodowisku deweloperskim
    app.UseSwaggerUI(); // W��czenie interfejsu u�ytkownika Swaggera w �rodowisku deweloperskim
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
