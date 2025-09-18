using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductMicroservice.Core.Domain.RepositoryContracts;
using ProductMicroservice.Infrastructure.DatabaseContext;
using ProductMicroservice.Infrastructure.Repositories;

namespace ProductMicroservice.Infrastructure;
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
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
