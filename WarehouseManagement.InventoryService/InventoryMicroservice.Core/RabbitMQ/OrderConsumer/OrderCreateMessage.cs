namespace InventoryMicroservice.Core.RabbitMQ.OrderConsumer;
public record OrderCreateMessage(List<OrderItemAddRequest> Items);
