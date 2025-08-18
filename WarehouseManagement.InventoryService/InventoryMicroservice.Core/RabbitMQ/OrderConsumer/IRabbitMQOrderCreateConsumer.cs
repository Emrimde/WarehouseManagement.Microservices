namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer
{
    public interface IRabbitMQOrderCreateConsumer
    {
        void Consume();
        void Dispose();
    }
}