using ProductService.Core.Domain.Entities;

namespace ProductService.Core.RepositoryContracts;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<bool> UpdateProductAsync(Product product, Guid id);
}
