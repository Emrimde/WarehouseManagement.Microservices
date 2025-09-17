using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Result;

namespace ProductMicroservice.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> AddCategory(CategoryAddRequest request);
        Task<Result<bool>> DeleteCategory(Guid id);
        Task<IEnumerable<CategoryResponse>> GetCategories();
        Task<Result<CategoryResponse>> GetCategoryById(Guid id);
        Task<Result<IEnumerable<ProductResponse>>> GetRelatedProductsWithCategoryById(Guid id);
        Task<Result<CategoryResponse>> UpdateCategory(Guid id, CategoryUpdateRequest category);
    }
}