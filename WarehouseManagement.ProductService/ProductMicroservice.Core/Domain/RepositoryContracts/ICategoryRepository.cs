using ProductMicroservice.Core.Domain.Entities;

namespace ProductMicroservice.Infrastructure.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> AddCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<int> GetCategoriesCountAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetRelatedProductsWithCategoryId(Guid id);
        Task<bool> UpdateCategory(Category category);
    }
}