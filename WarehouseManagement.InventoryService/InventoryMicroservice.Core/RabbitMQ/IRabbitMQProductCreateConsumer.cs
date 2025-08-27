namespace InventoryMicroservice.Core.RabbitMQ
{
    public interface IRabbitMQProductCreateConsumer
    {
        void Consume();
        void Dispose();
        void Initialize(int miliseconds = 3000);
    }
}