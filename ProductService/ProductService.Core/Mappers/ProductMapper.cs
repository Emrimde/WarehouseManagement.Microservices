using ProductService.Core.Domain.Entities;
using ProductService.Core.DTO;

namespace ProductService.Core.Mappers;
public static class ProductMapper
{
    public static ProductResponse ToProductResponse(this Product product)
    {
        return new ProductResponse
        (
             product.Id,
             product.Name,
             product.Description,
             product.StockKeepingUnit
        );
    }

    public static Product ToProduct(this ProductUpdateRequest product)
    {
        return new Product()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
        };
    }

    public static Product ToProduct(this ProductAddRequest product)
    {
        return new Product()
        {
            Name = product.Name,
            Description = product.Description,
            StockKeepingUnit = product.StockKeepingUnit,
            CategoryId = product.CategoryId,
            Manufacturer = product.Manufacturer,
        };
    }
}
