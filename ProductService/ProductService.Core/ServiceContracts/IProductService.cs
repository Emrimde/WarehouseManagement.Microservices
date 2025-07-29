using ProductService.Core.DTO;
using ProductService.Core.Result;

namespace ProductService.Core.ServiceContracts;
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetProductsAsync();
    Task<Result<ProductResponse>> GetProductByIdAsync(Guid id);
    Task<Result<ProductResponse>> UpdateProductAsync(ProductUpdateRequest product, Guid id);
}
