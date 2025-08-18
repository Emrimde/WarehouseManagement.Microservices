using OrderMicroservice.Core.Enums;

namespace OrderMicroservice.Core.DTO.OrderDTO;
public class OrderResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = default!; 
    public OrderStatus Status { get; set; }
}
