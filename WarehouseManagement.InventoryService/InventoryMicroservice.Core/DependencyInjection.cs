using InventoryMicroservice.Core.ServiceContracts;
using InventoryMicroservice.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryMicroservice.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IInventoryService, InventoryService>();
        return services;
    }
}
