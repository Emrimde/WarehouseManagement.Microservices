namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer;
public record OrderCreateMessage(List<OrderItemAddRequest> Items, string orderId, DateTime CreatedAt, string CustomerName, string OrderNumber);
