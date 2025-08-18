using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
namespace OrderMicroservice.Core.RabbitMQ;
public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _exchangeName;
    public RabbitMQPublisher()
    {
        _exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_ORDER_EXCHANGE")!;
        ConnectionFactory _connectionFactory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBIT_MQ_HOSTNAME")!,
            Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBIT_MQ_PORT"))!,
            Password = Environment.GetEnvironmentVariable("RABBIT_MQ_PASSWORD")!,
            UserName = Environment.GetEnvironmentVariable("RABBIT_MQ_USERNAME")!
        };

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }

    public void Publish<T>(string routingKey, T message)
    {
        string messageJson = JsonConvert.SerializeObject(message);
        byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);
        _channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, basicProperties: null, body: messageBodyInBytes);
    }
}
