using Microsoft.Extensions.DependencyInjection;
using ProductMicroservice.Core.RabbitMQ;
using ProductMicroservice.Core.ServiceContracts;
using ProductMicroservice.Core.Services;

namespace ProductMicroservice.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        _ = services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
        return services;
    }
}
