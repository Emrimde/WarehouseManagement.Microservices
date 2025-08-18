namespace InventoryMicroservice.Core.RabbitMQ;
public record OrderItemAddRequest(string SKU, int Quantity);
