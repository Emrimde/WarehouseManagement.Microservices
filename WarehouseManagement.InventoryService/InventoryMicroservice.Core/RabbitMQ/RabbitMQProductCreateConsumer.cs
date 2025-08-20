using InventoryMicroservice.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace InventoryMicroservice.Core.RabbitMQ;
public class RabbitMQProductCreateConsumer : IDisposable, IRabbitMQProductCreateConsumer
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQProductCreateConsumer> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly string _exchangeName;
    public RabbitMQProductCreateConsumer(
     ILogger<RabbitMQProductCreateConsumer> logger,
     IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_EXCHANGE")!;

        // debug: wypisz wartości
        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME")!;
        var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT")!;
        Console.WriteLine($"[DEBUG Consumer] Host={host}, Port={port}, Exchange={_exchangeName}");

        var factory = new ConnectionFactory
        {
            HostName = host,
            Port = int.Parse(port),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!,
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }


    public void Consume()
    {
        string routingKey = "product.create";
        string queueName = "inventory.product.create.queue";
    
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);

        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);


        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: routingKey);

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, args) =>
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("[LOG] - Message from product microservice gained");
                    var message = Encoding.UTF8.GetString(args.Body.ToArray());
                    var product = JsonConvert.DeserializeObject<ProductCreateMessage>(message);

                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IInventoryRepository>();

                    await repo.AddEmptyInventoryItemWithSkuAsync(product!.Sku); // After getting a message from product microservice i wanted to create an inventory item.
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message.");
                }
            });
        };

        _channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
