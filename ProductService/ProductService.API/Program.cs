using ProductService.Core;
using ProductService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load();

// Add services to the container.
builder.Services.AddControllers(); // dodanie kontroler�w do kontenera Dependency Injection 
builder.Services.AddEndpointsApiExplorer(); // dodanie eksploratora punkt�w ko�cowych
builder.Services.AddSwaggerGen(); // dodanie Swaggera do kontenera Dependency Injection
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = $"{Environment.GetEnvironmentVariable("REDIS_HOST")}:{Environment.GetEnvironmentVariable("REDIS_PORT")}";

});
builder.Services.AddInfrastructure();
builder.Services.AddCore();

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
