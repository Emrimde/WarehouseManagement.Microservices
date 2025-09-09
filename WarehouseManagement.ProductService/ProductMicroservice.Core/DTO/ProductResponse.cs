namespace ProductService.Core.DTO;
public record ProductResponse
(
    Guid Id,
    string Name,
    string Description, 
    string Sku,
    string CategoryName,
    Guid CategoryId,
    string Manufacturer,
    DateTime CreatedAt
);

