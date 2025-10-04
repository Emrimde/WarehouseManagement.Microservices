using ProductMicroservice.Core;
using ProductMicroservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = $"{Environment.GetEnvironmentVariable("REDIS_HOST")}:{Environment.GetEnvironmentVariable("REDIS_PORT")}";

});
builder.Services.AddInfrastructure();
builder.Services.AddCore();

var app = builder.Build();
app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
