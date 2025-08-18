using Microsoft.Extensions.Hosting;

namespace InventoryMicroservice.Core.RabbitMQ;
public class RabbitMQProductCreateHostedService : IHostedService
{
    private readonly IRabbitMQProductCreateConsumer _productCreateConsumer;
    public RabbitMQProductCreateHostedService(IRabbitMQProductCreateConsumer productCreateConsumer)
    {
        _productCreateConsumer = productCreateConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    { 
        _productCreateConsumer.Consume();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _productCreateConsumer.Dispose();
        return Task.CompletedTask;
    }
}
