using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Results;

namespace ProductMicroservice.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> AddCategoryAsync(CategoryAddRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteCategory(Guid id);
        Task<PagedResult<CategoryResponse>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Result<CategoryResponse>> GetCategoryAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<ProductResponse>>> GetRelatedProductsWithCategoryById(Guid id);
        Task<Result> UpdateCategoryNameAsync(Guid id, CategoryUpdateRequest request, CancellationToken cancellatioToken);
    }
}