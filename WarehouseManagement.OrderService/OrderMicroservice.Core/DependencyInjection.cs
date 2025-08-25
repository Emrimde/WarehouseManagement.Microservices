using Microsoft.Extensions.DependencyInjection;
using OrderMicroservice.Core.RabbitMQ;
using OrderMicroservice.Core.ServiceContracts;
using OrderMicroservice.Core.Services;
using StackExchange.Redis;

namespace OrderMicroservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            services.AddStackExchangeRedisCache(options => options.Configuration = $"{Environment.GetEnvironmentVariable("REDIS_HOST")}:{Environment.GetEnvironmentVariable("REDIS_PORT")}");
            
            return services;
        }
    }
}
