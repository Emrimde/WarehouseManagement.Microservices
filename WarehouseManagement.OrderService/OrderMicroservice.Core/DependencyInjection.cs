using Microsoft.Extensions.DependencyInjection;
using OrderMicroservice.Core.ServiceContracts;
using OrderMicroservice.Core.Services;

namespace OrderMicroservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
