using ProductMicroservice.Core.Domain.Entities;

namespace ProductMicroservice.Infrastructure.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken);
        Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken);
        Task<int> GetCategoriesCountAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Category?> GetCategoryAsync(Guid id,CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetRelatedProductsWithCategoryId(Guid id);
        Task<bool> UpdateCategoryNameAsync(Guid id, string name,CancellationToken cancellationToken);
        Task<bool> IsCategoryNameUnique(string name, CancellationToken cancellationToken);
    }
}