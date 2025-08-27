using InventoryMicroservice.Core.RabbitMQ.PickingPublisher;
using InventoryMicroservice.Core.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer;
public class RabbitMQOrderCreateConsumer : IDisposable, IRabbitMQOrderCreateConsumer
{
    private  IModel _channel = default!;
    private  IConnection _connection = default!;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _exchangeName;
    private readonly ILogger<RabbitMQOrderCreateConsumer> _logger;
    public RabbitMQOrderCreateConsumer(IServiceScopeFactory scopeFactory, ILogger<RabbitMQOrderCreateConsumer> logger)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_ORDER_EXCHANGE")!;
    }

    public void Initialize(int miliseconds = 3000)
    {
        bool connected = false;
        int attempt = 0;
        while (!connected)
        {
            try
            {
                SetupConnectionAndChannel();
                _logger.LogInformation("Connection with RabbitMQ established");
                connected = true;

            }
            catch (Exception)
            {
                attempt++;
                _logger.LogInformation($"Connection with RabbitMQ failed. Attempt: {attempt}");
                Thread.Sleep((int)Math.Pow(miliseconds,attempt));
            }
        }
    }

    public void SetupConnectionAndChannel()
    {
        string host = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME")!;
        int port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT")!);
        string username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!;
        string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!;

        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            Port = port,
            HostName = host,
            UserName = username,
            Password = password
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Consume()
    {
        string routingKey = "order.created";
        string queueName = "inventory.order.create.queue";

        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);

        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);


        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: routingKey);

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) =>
        {
            try
            {
                _logger.LogInformation("[LOG] - Message from order microservice gained");
                string message = Encoding.UTF8.GetString(args.Body.ToArray());
                OrderCreateMessage? order = JsonConvert.DeserializeObject<OrderCreateMessage>(message);

                using var scope = _scopeFactory.CreateScope();
                var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryReservationService>();

                if (order == null) throw new Exception("Order is null");

                _logger.LogInformation("[LOG] - publish inventory.reservated");
                await inventoryService.ProcessOrderAsync(new PickingMessage(order.Items, order.orderId, order.CreatedAt, order.CustomerName, order.OrderNumber));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message.");
            }
        };

        _channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);
    }
    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
