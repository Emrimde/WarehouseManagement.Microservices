using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Result;

namespace ProductMicroservice.Core.ServiceContracts;
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetProductsAsync();
    Task<Result<ProductResponse>> GetProductByIdAsync(Guid id);
    Task<Result<ProductResponse>> GetProductBySkuAsync(string sku);
    Task<Result<ProductUpdateRequest>> UpdateProductAsync(ProductUpdateRequest product, Guid id);
    Task<Result<ProductResponse>> AddProductAsync(ProductAddRequest product);
    Task<Result<ProductResponse>> DeleteProduct(Guid id);
    Task<PagedResult<ProductResponse>> GetProductsPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
}
