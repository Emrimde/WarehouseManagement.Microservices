using ProductService.Core.Domain.Entities;
using ProductService.Core.DTO;

namespace ProductService.Core.Mappers;
public static class ProductMapper
{
    public static ProductResponse ToProductResponse(this Product product)
    {
        return new ProductResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
        };
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
    
}
