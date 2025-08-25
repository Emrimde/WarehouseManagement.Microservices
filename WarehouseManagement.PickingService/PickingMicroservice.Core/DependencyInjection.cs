using Microsoft.Extensions.DependencyInjection;
using PickingMicroservice.Core.RabbitMQ.InventoryConsumer;
using PickingMicroservice.Core.Service;
using PickingMicroservice.Core.ServiceContracts;

namespace PickingMicroservice.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IPickingService, PickingService>();
        services.AddSingleton<IRabbitMQInventoryConsumer, RabbitMQInventoryConsumer>();
        services.AddHostedService<RabbitMQInventoryHostedService>();

        services.AddStackExchangeRedisCache(options => options.Configuration = $"{Environment.GetEnvironmentVariable("REDIS_HOST")}:{Environment.GetEnvironmentVariable("REDIS_PORT")}");
        return services;
        
    }
}
