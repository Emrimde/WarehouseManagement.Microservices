using InventoryMicroservice.Core.RabbitMQ;
using InventoryMicroservice.Core.RabbitMQ.OrderConsumer;
using InventoryMicroservice.Core.RabbitMQ.PickingPublisher;
using InventoryMicroservice.Core.ServiceContracts;
using InventoryMicroservice.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryMicroservice.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IInventoryReservationService, InventoryReservationService>();
        services.AddSingleton<IRabbitMQProductCreateConsumer, RabbitMQProductCreateConsumer>();
        services.AddSingleton<IRabbitMQOrderCreateConsumer, RabbitMQOrderCreateConsumer>();
        services.AddTransient<IRabbitMQPickingPublisher, RabbitMQPickingPublisher>();
        services.AddHostedService<RabbitMQProductCreateHostedService>();
        services.AddHostedService<RabbitMQOrderCreateHostedService>();

        services.AddStackExchangeRedisCache(options => options.Configuration = $"{Environment.GetEnvironmentVariable("REDIS_HOST")}:{Environment.GetEnvironmentVariable("REDIS_PORT")}");

        return services;
    }
}
