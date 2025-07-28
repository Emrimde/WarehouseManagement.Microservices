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
builder.Services.AddControllers(); // dodanie kontrolerów do kontenera Dependency Injection 
builder.Services.AddEndpointsApiExplorer(); // dodanie eksploratora punktów koñcowych
builder.Services.AddSwaggerGen(); // dodanie Swaggera do kontenera Dependency Injection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Sprawdzenie, czy œrodowisko to œrodowisko deweloperskie
{
    app.UseSwagger(); // W³¹czenie Swaggera w œrodowisku deweloperskim
    app.UseSwaggerUI(); // W³¹czenie interfejsu u¿ytkownika Swaggera w œrodowisku deweloperskim
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
