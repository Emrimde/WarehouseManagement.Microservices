using ProductService.Core.Domain.Entities;
using ProductService.Core.DTO;

namespace ProductService.Core.Mappers;
public static class ProductMapper
{
    public static ProductResponse ToProductResponse(this Product response)
    {
        return new ProductResponse()
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
        };
    }
}
