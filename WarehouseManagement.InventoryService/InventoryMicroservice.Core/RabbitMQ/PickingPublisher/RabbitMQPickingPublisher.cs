using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InventoryMicroservice.Core.RabbitMQ.PickingPublisher;
public class RabbitMQPickingPublisher : IDisposable, IRabbitMQPickingPublisher
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _exchangeName;
    public RabbitMQPickingPublisher()
    {
        _exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_INVENTORY_EXCHANGE")!;
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"),
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"),
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT")!)
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }

    public void Publish<T>(string routingKey, T message)
    {
        string messageJson = JsonSerializer.Serialize(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);
        _channel.BasicPublish(basicProperties: null, exchange: _exchangeName, body: messageBodyInBytes, routingKey: routingKey);
    }
}
