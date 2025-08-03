using ProductService.Core;
using ProductService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load();

// Add services to the container.
builder.Services.AddControllers(); // dodanie kontrolerów do kontenera Dependency Injection 
builder.Services.AddEndpointsApiExplorer(); // dodanie eksploratora punktów koñcowych
builder.Services.AddSwaggerGen(); // dodanie Swaggera do kontenera Dependency Injection
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = $"{Environment.GetEnvironmentVariable("REDIS_HOST")}:{Environment.GetEnvironmentVariable("REDIS_PORT")}";

});
builder.Services.AddInfrastructure();
builder.Services.AddCore();

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
