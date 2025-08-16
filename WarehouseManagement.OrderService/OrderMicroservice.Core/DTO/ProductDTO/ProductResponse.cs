namespace OrderMicroservice.Core.DTO.ProductDTO;
public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
}
