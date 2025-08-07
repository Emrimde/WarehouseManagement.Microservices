namespace InventoryMicroservice.Core.RabbitMQ
{
    public interface IRabbitMQProductCreateConsumer
    {
        void Consume();
        void Dispose();
    }
}