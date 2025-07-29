namespace ProductService.Core.DTO;
public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}

