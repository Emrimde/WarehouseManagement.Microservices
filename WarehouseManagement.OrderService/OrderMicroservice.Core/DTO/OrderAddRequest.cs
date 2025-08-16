namespace OrderMicroservice.Core.DTO;
public record OrderAddRequest
(
    List<OrderItemAddRequest> Items,
    string CustomerName,
    string CustomerEmail

);
