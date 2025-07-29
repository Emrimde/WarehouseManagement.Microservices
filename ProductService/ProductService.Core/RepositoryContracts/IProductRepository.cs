using ProductService.Core.Domain.Entities;

namespace ProductService.Core.RepositoryContracts;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product?> GetProductById(Guid id);
}
