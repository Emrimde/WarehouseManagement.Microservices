using OrderMicroservice.Core.Enums;

namespace OrderMicroservice.Core.Domain.Entities;
public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = default!; // identifier
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public OrderStatus Status { get; set; } 
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public ICollection<OrderItem> Items { get; set; } = [];
}

