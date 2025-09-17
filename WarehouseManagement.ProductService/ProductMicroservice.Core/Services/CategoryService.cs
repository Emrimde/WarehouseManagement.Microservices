using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Core.Result;
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
    public async Task<IEnumerable<CategoryResponse>> GetCategories()
    {
        IEnumerable<Category> categories = await _categoryRepo.GetAllCategoriesAsync();
        return categories.Select(item => item.ToCategoryResponse());
    }

    public async Task<Result<CategoryResponse>> GetCategoryById(Guid id)
    {
        Category? category = await _categoryRepo.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return Result<CategoryResponse>.Failure("Category is not found", StatusCode.NotFound);
        }
        return Result<CategoryResponse>.Success(category.ToCategoryResponse());
    }

    public async Task<Result<CategoryResponse>> AddCategory(CategoryAddRequest request)
    {
        Category category = request.ToCategory();
        Category? addedCategory = await _categoryRepo.AddCategoryAsync(category);
        if (addedCategory == null)
        {
            return Result<CategoryResponse>.Failure("Category not added", StatusCode.NotFound);
        }
        return Result<CategoryResponse>.Success(category.ToCategoryResponse());
    }

    public async Task<Result<bool>> DeleteCategory(Guid id)
    {

        bool isDeleted = await _categoryRepo.DeleteCategoryAsync(id);
        if (!isDeleted)
        {
            return Result<bool>.Failure("Category not deleted", StatusCode.BadRequest);
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
            return Result<CategoryResponse>.Failure("Error: Id of the category is not equal to id in query", StatusCode.BadRequest);
        }

        bool isModified = await _categoryRepo.UpdateCategory(category.ToCategory());

        if (!isModified)
        {
            return Result<CategoryResponse>.Failure("Error: Unable to update category",StatusCode.NotFound);
        }

        return Result<CategoryResponse>.SuccessResult("Successfully updated");
    }
}
