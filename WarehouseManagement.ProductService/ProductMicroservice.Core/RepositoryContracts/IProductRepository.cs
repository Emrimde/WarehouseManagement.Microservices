using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.DTO;

namespace ProductMicroservice.Core.RepositoryContracts;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<bool> UpdateProductAsync(Product product, Guid id);
    Task<Product> AddProductAsync(Product product);
    Task<Product?> GetProductBySkuAsync(string sku);
    Task<bool> IsProductValid(Product product);
    Task<bool> DeleteProduct(Guid id);
    Task<IEnumerable<Product>> GetProductsPageProjectedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> GetActiveProductsCountAsync(CancellationToken cancellationToken);
}
