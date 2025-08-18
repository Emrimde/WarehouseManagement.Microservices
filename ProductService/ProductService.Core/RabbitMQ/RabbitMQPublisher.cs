using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ProductService.Core.RabbitMQ;
public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    private readonly string _exchangeName;
    public RabbitMQPublisher()
    {
        _exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_ProductExchange")!;
        ConnectionFactory _connectionFactory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME")!,
            Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RabbitMQ_PORT"))!,
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!,
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!
        };

        _connection =  _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

    }
    
    public void Publish<T>(string routingKey, T message)
    {
        string messageJson = JsonConvert.SerializeObject(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);

         _channel.BasicPublish(
            exchange: _exchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: messageBodyInBytes);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
