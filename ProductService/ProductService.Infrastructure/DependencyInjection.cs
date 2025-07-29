using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.RepositoryContracts;
using ProductService.Infrastructure.DatabaseContext;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        string host = Environment.GetEnvironmentVariable("HOST")!;
        string username = Environment.GetEnvironmentVariable("USERNAME")!;
        string password = Environment.GetEnvironmentVariable("PASSWORD")!;
        string database = Environment.GetEnvironmentVariable("DATABASE")!;

        string connectionString = $"Host={host};Username={username};Password={password};Database={database}";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}
