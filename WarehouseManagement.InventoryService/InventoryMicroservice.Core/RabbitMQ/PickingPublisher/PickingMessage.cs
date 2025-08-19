namespace InventoryMicroservice.Core.RabbitMQ.PickingPublisher;
public record PickingMessage(List<OrderItemAddRequest> Items, Guid orderId, DateTime CreatedAt);
