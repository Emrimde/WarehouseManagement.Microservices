namespace ProductService.Core.RabbitMQ;
public interface IRabbitMQPublisher
{
    Task Publish<T>(string routingKey, T message);
    Task InitAsync();
}
