namespace PickingMicroservice.Core.RabbitMQ.InventoryConsumer;
public record PickingMessage(List<OrderItemAddRequest> Items, string orderId, DateTime CreatedAt, string CustomerName, string OrderNumber);
