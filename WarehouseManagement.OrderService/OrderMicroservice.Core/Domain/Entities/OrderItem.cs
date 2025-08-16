namespace OrderMicroservice.Core.Domain.Entities;
public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public string ProductName { get; set; } = default!; 
    public decimal UnitPrice { get; set; } 
    public int Quantity { get; set; }
}
