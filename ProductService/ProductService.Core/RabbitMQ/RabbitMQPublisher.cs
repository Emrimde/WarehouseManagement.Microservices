using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ProductService.Core.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private IChannel _channel = default!;
        public RabbitMQPublisher()
        {
        }
        public async Task InitAsync()
        {
            ConnectionFactory _connectionFactory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RabbitMQ_HOSTNAME")!,
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RabbitMQ_PORT"))!,
                Password = Environment.GetEnvironmentVariable("RabbitMQ_PASSWORD")!,
                UserName = Environment.GetEnvironmentVariable("RabbitMQ_USERNAME")!
            };

            IConnection connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: "products.exchange", type: ExchangeType.Direct, durable: true);
        }

        public async Task Publish<T>(string routingKey, T message)
        {
            string messageJson = JsonConvert.SerializeObject(message);
            byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

            BasicProperties props = new BasicProperties
            {
                Persistent = true
            };

            await _channel.BasicPublishAsync(
                exchange: "products.exchange",
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: messageBodyInBytes);
        }
    }
}
