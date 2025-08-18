namespace OrderMicroservice.Core.DTO.OrderDTO;
public record OrderAddRequest
(
    List<OrderItemAddRequest> Items,
    string CustomerName,
    string CustomerEmail
);
