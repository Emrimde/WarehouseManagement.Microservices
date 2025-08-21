using OrderMicroservice.Core.DTO.OrderDTO;
namespace OrderMicroservice.Core.RabbitMQ;
public record OrderCreateMessage(List<OrderItemAddRequest> Items, string orderId, DateTime CreatedAt, string CustomerName, string OrderNumber);
