namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer;
public record OrderCreateMessage(List<OrderItemAddRequest> Items, Guid orderId, DateTime CreatedAt);
