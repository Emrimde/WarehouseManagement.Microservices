namespace PickingMicroservice.Core.RabbitMQ.InventoryConsumer
{
    public interface IRabbitMQInventoryConsumer
    {
        void Consume();
        void Dispose();
    }
}