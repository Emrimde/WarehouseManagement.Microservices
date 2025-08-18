using Microsoft.Extensions.Hosting;

namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer;
public class RabbitMQOrderCreateHostedService : IHostedService
{
    private readonly IRabbitMQOrderCreateConsumer _orderCreateConsumer;
    public RabbitMQOrderCreateHostedService(IRabbitMQOrderCreateConsumer orderCreateConsumer)
    {
        _orderCreateConsumer = orderCreateConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _orderCreateConsumer.Consume();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _orderCreateConsumer.Dispose();
        return Task.CompletedTask;
    }
}
