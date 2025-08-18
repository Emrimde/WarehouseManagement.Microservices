using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderMicroservice.Core.Domain.RepositoryContracts;
using OrderMicroservice.Infrastructure.DatabaseContext;
using OrderMicroservice.Infrastructure.Repositories;

namespace OrderMicroservice.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        string host = Environment.GetEnvironmentVariable("DB_HOST")!;
        string username = Environment.GetEnvironmentVariable("DB_USERNAME")!;
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD")!;
        string port = Environment.GetEnvironmentVariable("DB_PORT")!;
        string db_name = Environment.GetEnvironmentVariable("DB_NAME")!;

        string connectionString = $"Host={host};Username={username};Port={port};Database={db_name};Password={password}";

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        return services;
    }
}
