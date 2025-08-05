using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Infrastructure.DatabaseContext;
using InventoryMicroservice.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryMicroservice.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        string host = Environment.GetEnvironmentVariable("HOST")!;
        string password = Environment.GetEnvironmentVariable("PASSWORD")!;
        string port = Environment.GetEnvironmentVariable("PORT")!;
        string username = Environment.GetEnvironmentVariable("USERNAME")!;
        string database = Environment.GetEnvironmentVariable("DATABASE")!;

        string connectionString = $"Host={host};Password={password};Username={username};Port={port};Database={database}";

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}
