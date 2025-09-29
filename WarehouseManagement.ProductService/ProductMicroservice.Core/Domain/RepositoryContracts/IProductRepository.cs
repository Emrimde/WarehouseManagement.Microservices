using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.Enums;

namespace ProductMicroservice.Core.Domain.RepositoryContracts;
public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateProductAsync(Product product, Guid id,CancellationToken cancellationToken);
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetProductBySkuAsync(string sku);
   
    Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsPageProjectedAsync(int page, int pageSize, string? name, ProductSearchCategoriesEnum category, CancellationToken cancellationToken);
    Task<int> GetActiveProductsCountAsync(CancellationToken cancellationToken);
    Task<bool> IsCategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<bool> IsSkuExistsAsync(string sku, CancellationToken cancellationToken);
    Task<bool> ExistsByNameInCategoryAsync(string name, Guid categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> SearchForProduct(ProductSearchCategoriesEnum category, string name, CancellationToken cancellationToken);
}
