using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Results;

namespace ProductMicroservice.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> AddCategory(CategoryAddRequest request);
        Task<Result<bool>> DeleteCategory(Guid id);
        Task<PagedResult<CategoryResponse>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Result<CategoryResponse>> GetCategoryById(Guid id);
        Task<Result<IEnumerable<ProductResponse>>> GetRelatedProductsWithCategoryById(Guid id);
        Task<Result<CategoryResponse>> UpdateCategory(Guid id, CategoryUpdateRequest category);
    }
}