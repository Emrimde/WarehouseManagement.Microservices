namespace ProductService.Core.Domain.Entities;
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string StockKeepingUnit { get; set; } = default!;
    public Guid CategoryId { get; set; } = default!;
    public Category? Category { get; set; }
    public string Manufacturer { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;
    public DateTime? DeletedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; } 
    public bool IsActive { get; set; } = default!;
}
