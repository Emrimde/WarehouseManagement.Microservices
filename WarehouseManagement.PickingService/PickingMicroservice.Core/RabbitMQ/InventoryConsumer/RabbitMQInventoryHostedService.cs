using Microsoft.Extensions.Hosting;

namespace PickingMicroservice.Core.RabbitMQ.InventoryConsumer;
public class RabbitMQInventoryHostedService : IHostedService
{
    private readonly IRabbitMQInventoryConsumer _inventoryConsumer;
    public RabbitMQInventoryHostedService(IRabbitMQInventoryConsumer inventoryConsumer)
    {
        _inventoryConsumer = inventoryConsumer;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _inventoryConsumer.Initialize(3000);
        _inventoryConsumer.Consume();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _inventoryConsumer.Dispose();
        return Task.CompletedTask;
    }
}
