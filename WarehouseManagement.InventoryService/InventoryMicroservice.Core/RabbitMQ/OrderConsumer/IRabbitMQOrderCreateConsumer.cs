namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer
{
    public interface IRabbitMQOrderCreateConsumer
    {
        void Consume();
        void Dispose();
        void Initialize(int miliseconds = 3000);
    }
}