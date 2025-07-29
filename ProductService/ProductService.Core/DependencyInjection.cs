using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.ServiceContracts;

namespace ProductService.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService.Core.Services.ProductService>();
            return services;
        }
    }
}
