using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PickingMicroservice.Infrastructure.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PickingMicroservice.Core.RabbitMQ.InventoryConsumer;
public class RabbitMQInventoryConsumer : IRabbitMQInventoryConsumer, IDisposable
{
    private readonly ILogger<RabbitMQInventoryConsumer> _logger;
    private IModel _channel = default!;
    private IConnection _connection = default!;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _exchangeName;
    public RabbitMQInventoryConsumer(ILogger<RabbitMQInventoryConsumer> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_INVENTORY_EXCHANGE")!;
        _scopeFactory = scopeFactory;
    }

    public void Initialize(int miliseconds = 3000)
    {
        bool connected = false;
        int attempt = 0;
        while (!connected) {
            try
            {
                SetupConnectionAndChannel();
                _logger.LogInformation("Connection to RabbitMQ successfully established");
                connected = true;
            }
            catch(Exception ex)
            {
                attempt++;
                _logger.LogError($"Connection to RabbitMQ failed. Attempt:{attempt}");
                Thread.Sleep(3000);
            }
        
        }
    }

    private void SetupConnectionAndChannel()
    {
        string host = Environment.GetEnvironmentVariable("RABBITMQ_HOST")!;
        string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!;
        string username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!;
        string port = Environment.GetEnvironmentVariable("RABBITMQ_PORT")!;
        ConnectionFactory _connectionFactory = new ConnectionFactory()
        {
            HostName = host,
            Port = int.Parse(port),
            Password = password,
            UserName = username
        };
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Consume()
    {
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false);
        string queueName = "picking.inventory.reservated.queue";
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queueName, _exchangeName, "inventory.reservated");

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) =>
        {
            try
            {
                _logger.LogInformation("[LOG] Message from inventory received");
                string message = Encoding.UTF8.GetString(args.Body.ToArray());
                PickingMessage? pickingMessage = JsonConvert.DeserializeObject<PickingMessage>(message);
                if (pickingMessage == null)
                {
                    throw new Exception("Picking message is null");
                }
                using IServiceScope scope = _scopeFactory.CreateScope();
                IPickingRepository repo = scope.ServiceProvider.GetRequiredService<IPickingRepository>();
                await repo.CreatePickTask(pickingMessage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message.");
            }
        };

        _channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
