namespace InventoryMicroservice.Core.RabbitMQ.PickingPublisher
{
    public interface IRabbitMQPickingPublisher
    {
        void Publish<T>(string routingKey, T message);
    }
}