using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Enums;
using ProductMicroservice.Core.Results;

namespace ProductMicroservice.Core.ServiceContracts;
public interface IProductService
{
    Task<Result<ProductResponse>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<ProductResponse>> GetProductBySkuAsync(string sku);
    Task<Result> UpdateProductAsync(ProductUpdateRequest product, Guid id, CancellationToken cancellationToken);
    Task<Result<ProductCreateResponse>> AddProductAsync(ProductAddRequest product, CancellationToken cancellationToken);
    Task<Result> DeleteProductAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<ProductResponse>> GetProductsPagedAsync(int page, int pageSize, string? name, ProductSearchCategoriesEnum category, CancellationToken cancellationToken, bool showActive);
    Task<Result> PermanentDeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Result> RestoreProductAsync(Guid id, CancellationToken cancellationToken);
}
