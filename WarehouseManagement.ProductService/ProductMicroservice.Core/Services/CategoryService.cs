using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Enums;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Core.Results;
using ProductMicroservice.Core.ServiceContracts;
using ProductMicroservice.Infrastructure.Repositories;
using System.Text.Json;

namespace ProductMicroservice.Core.Services;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IDistributedCache _cache;
    private readonly ILogger<CategoryService> _logger;
    public CategoryService(ICategoryRepository categoryRepo, IDistributedCache cache, ILogger<CategoryService> logger)
    {
        _categoryRepo = categoryRepo;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<CategoryResponse>> GetCategoryAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return Result<CategoryResponse>.Failure("Invalid category id", StatusCodeEnum.BadRequest);
        }

        string cacheKey = $"category:{id}";
        byte[]? cachedCategory = await _cache.GetAsync(cacheKey, cancellationToken);
        if (cachedCategory != null && cachedCategory.Length > 0)
        {
            try
            {

                CategoryResponse? categoryFromCache = JsonSerializer.Deserialize<CategoryResponse>(cachedCategory);

                if (categoryFromCache != null)
                {
                    return Result<CategoryResponse>.Success(categoryFromCache);
                }
            }
            catch (JsonException exception)
            {
                _logger.LogError(exception, "Failed to deserialize category from cache (key={CacheKey})", cacheKey);
            }
        }

        Category? category = await _categoryRepo.GetCategoryAsync(id, cancellationToken);
        if (category == null)
        {
            return Result<CategoryResponse>.Failure("Category is not found", StatusCodeEnum.NotFound);
        }
        CategoryResponse categoryResponse = category.ToCategoryResponse();

        byte[] categoryInBytes = JsonSerializer.SerializeToUtf8Bytes(categoryResponse);
        await _cache.SetAsync(cacheKey, categoryInBytes,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }, cancellationToken);

        return Result<CategoryResponse>.Success(categoryResponse);
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

    public async Task<Result> UpdateCategoryNameAsync(Guid id, CategoryUpdateRequest request,CancellationToken cancellationToken)
    {
        string name = request.Name?.Trim() ?? string.Empty;
        if(name.Length < 3 || name.Length > 30)
        {
            return Result.Failure("Name must be 3-30 characters", StatusCodeEnum.BadRequest);
        }
        if(await _categoryRepo.IsCategoryNameUnique(name,cancellationToken))
        {
            return Result.Failure("Category isn't unique",StatusCodeEnum.Conflict); 
        }
        bool isModified = await _categoryRepo.UpdateCategoryNameAsync(id,name, cancellationToken);

        if (!isModified)
        {
            return Result.Failure("Category not found", StatusCodeEnum.NotFound);
        }
        try
        {
            await _cache.RemoveAsync($"category:{id}", cancellationToken);
        }
        catch(Exception cacheError) 
        {
            _logger.LogWarning(cacheError, "Failed to remove cache for category {CategoryId}", id);

        }

        return Result.Success();
    }

    public async Task<PagedResult<CategoryResponse>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categoryList = await _categoryRepo.GetCategoriesAsync(page, pageSize, cancellationToken);

        int total = await _categoryRepo.GetCategoriesCountAsync(cancellationToken);

        return PagedResult<CategoryResponse>.Create
            (categoryList.Select(item => item.ToCategoryResponse()), page, pageSize);
    }
}
