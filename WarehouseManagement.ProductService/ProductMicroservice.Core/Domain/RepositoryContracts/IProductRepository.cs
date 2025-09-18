using ProductMicroservice.Core.Domain.Entities;

namespace ProductMicroservice.Core.Domain.RepositoryContracts;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateProductAsync(Product product, Guid id,CancellationToken cancellationToken);
    Task<Product> AddProductAsync(Product product);
    Task<Product?> GetProductBySkuAsync(string sku);
    Task<bool> IsProductValid(Product product);
    Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsPageProjectedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> GetActiveProductsCountAsync(CancellationToken cancellationToken);
}
