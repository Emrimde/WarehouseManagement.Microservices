namespace PickingMicroservice.Core.RabbitMQ.InventoryConsumer;
public record OrderItemAddRequest(string SKU, int Quantity);
