using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.RabbitMQ;
using ProductService.Core.ServiceContracts;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace ProductService.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService.Core.Services.ProductService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
