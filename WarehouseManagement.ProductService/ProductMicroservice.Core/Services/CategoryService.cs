using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Enums;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Core.Results;
using ProductMicroservice.Core.ServiceContracts;
using ProductMicroservice.Infrastructure.Repositories;

namespace ProductMicroservice.Core.Services;
public class CategoryService : ICategoryService
{
    public readonly ICategoryRepository _categoryRepo;
    public CategoryService(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryResponse>> GetCategoryById(Guid id)
    {
        Category? category = await _categoryRepo.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return Result<CategoryResponse>.Failure("Category is not found", StatusCodeEnum.NotFound);
        }
        return Result<CategoryResponse>.Success(category.ToCategoryResponse());
    }

    public async Task<Result<CategoryResponse>> AddCategory(CategoryAddRequest request)
    {
        Category category = request.ToCategory();
        Category? addedCategory = await _categoryRepo.AddCategoryAsync(category);
        if (addedCategory == null)
        {
            return Result<CategoryResponse>.Failure("Category not added", StatusCodeEnum.NotFound);
        }
        return Result<CategoryResponse>.Success(category.ToCategoryResponse());
    }

    public async Task<Result<bool>> DeleteCategory(Guid id)
    {

        bool isDeleted = await _categoryRepo.DeleteCategoryAsync(id);
        if (!isDeleted)
        {
            return Result<bool>.Failure("Category not deleted", StatusCodeEnum.BadRequest);
        }
        return Result<bool>.Success(isDeleted);
    }

    public async Task<Result<IEnumerable<ProductResponse>>> GetRelatedProductsWithCategoryById(Guid id)
    {
        IEnumerable<Product>? productsList = await _categoryRepo.GetRelatedProductsWithCategoryId(id);
       
        return Result<IEnumerable<ProductResponse>>.Success(productsList.Select(item => item.ToProductResponse()));
    }

    public async Task<Result<CategoryResponse>> UpdateCategory(Guid id, CategoryUpdateRequest category)
    {
        if(category.Id != id)
        {
            return Result<CategoryResponse>.Failure("Error: Id of the category is not equal to id in query", StatusCodeEnum.BadRequest);
        }

        bool isModified = await _categoryRepo.UpdateCategory(category.ToCategory());

        if (!isModified)
        {
            return Result<CategoryResponse>.Failure("Error: Unable to update category",StatusCodeEnum.NotFound);
        }

        return Result<CategoryResponse>.SuccessResult("Successfully updated");
    }

    public async Task<PagedResult<CategoryResponse>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categoryList = await _categoryRepo.GetCategoriesAsync(page, pageSize,cancellationToken);

        int total = await _categoryRepo.GetCategoriesCountAsync(cancellationToken);

        return PagedResult<CategoryResponse>.Create
            (categoryList.Select(item => item.ToCategoryResponse()),page,pageSize);
    }
}
