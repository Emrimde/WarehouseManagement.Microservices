using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PickingMicroservice.Infrastructure.DatabaseContext;

namespace PickingMicroservice.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        string host = Environment.GetEnvironmentVariable("DB_HOST")!;
        string port = Environment.GetEnvironmentVariable("DB_PORT")!;
        string username = Environment.GetEnvironmentVariable("DB_USERNAME")!;
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD")!;
        string database = Environment.GetEnvironmentVariable("DB_NAME")!;

        //string connectionString = $"Host={host};Port={port};Password={password};Username={username};Database={database}";
        string connectionString = $"Host=localhost;Port=5432;Password=admin;Username=postgres;Database=PickingDb";

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}
