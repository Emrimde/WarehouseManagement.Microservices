using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.Domain.RepositoryContracts;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Enums;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Core.RabbitMQ;
using ProductMicroservice.Core.Results;
using ProductMicroservice.Core.ServiceContracts;
using System.Text.Json;


namespace ProductMicroservice.Core.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly IDistributedCache _cache;
    private readonly IRabbitMQPublisher _publisher;
    private readonly ILogger<ProductService> _logger;
    public ProductService(IProductRepository productRepo, IDistributedCache cache, IRabbitMQPublisher publisher, ILogger<ProductService> logger)
    {
        _productRepo = productRepo;
        _cache = cache;
        _publisher = publisher;
        _logger = logger;
    }
    public async Task<Result<ProductResponse>> AddProductAsync(ProductAddRequest request, CancellationToken cancellationToken)
    {
        Product product = request.ToProduct();
        product.StockKeepingUnit.ToUpper();

        if (!await _productRepo.IsCategoryExistsAsync(product.CategoryId, cancellationToken))
        {
            return Result<ProductResponse>.Failure("Category doesn't exist", StatusCodeEnum.BadRequest);
        }
        if (await _productRepo.IsSkuExistsAsync(product.StockKeepingUnit, cancellationToken))
        {
            return Result<ProductResponse>.Failure("Failed to create product: product with specific sku exists ", StatusCodeEnum.Conflict);
        }
        if (await _productRepo.ExistsByNameInCategoryAsync(product.Name, product.CategoryId, cancellationToken))
        {
            return Result<ProductResponse>.Failure("Failed to create product: Product with this name and category exists", StatusCodeEnum.Conflict);
        }

        Product newProduct = await _productRepo.AddProductAsync(product, cancellationToken);

        _publisher.Publish("product.create", new ProductCreateMessage(newProduct.StockKeepingUnit, newProduct.Name));

        await _cache.RemoveAsync("products:all");

        return Result<ProductResponse>.Success(newProduct.ToProductResponse());
    }

    public async Task<Result> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return Result.Failure("Invalid id", StatusCodeEnum.BadRequest);
        }

        try
        {
            bool isDeleted = await _productRepo.DeleteProduct(id, cancellationToken);

            if (!isDeleted)
            {
                return Result.Failure("The product don't exist", StatusCodeEnum.NotFound);
            }
            try
            {
                await _cache.RemoveAsync("products:all",cancellationToken);
                await _cache.RemoveAsync($"products:{id}", cancellationToken);
            }
            catch (Exception cacheEx)
            {
                _logger.LogWarning(cacheEx, "Failed to remove cache");
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }

        catch (Exception exception) {

            _logger.LogError(exception, "Unexpected error deleting product {ProductId}", id);
            return Result.Failure("Unexpected error while deleting product", StatusCodeEnum.InternalServerError);
        }


        return Result.Success("Deleted succesfully!");
    }

    public async Task<Result<ProductResponse>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        string cacheKey = $"product:{id}";
        byte[]? cachedProduct = await _cache.GetAsync(cacheKey, cancellationToken);

        if (cachedProduct != null)
        {
            ProductResponse? productFromCache = JsonSerializer.Deserialize<ProductResponse>(cachedProduct);
            return Result<ProductResponse>.Success(productFromCache!);
        }

        Product? product = await _productRepo.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return Result<ProductResponse>.Failure("Product not found", StatusCodeEnum.NotFound);
        }

        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(product.ToProductResponse());
        await _cache.SetAsync(cacheKey, bytes, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)), cancellationToken);

        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<Result<ProductResponse>> GetProductBySkuAsync(string sku)
    {
        if (string.IsNullOrEmpty(sku))
        {
            return Result<ProductResponse>.Failure("Incorrect id", StatusCodeEnum.BadRequest);
        }

        string cacheKey = $"product:{sku}";
        string? cachedProduct = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedProduct))
        {
            ProductResponse? productFromCache = JsonSerializer.Deserialize<ProductResponse>(cachedProduct);
            return Result<ProductResponse>.Success(productFromCache!);
        }

        Product? product = await _productRepo.GetProductBySkuAsync(sku);
        if (product == null)
        {
            return Result<ProductResponse>.Failure("Product not found", StatusCodeEnum.NotFound);
        }

        string productJson = JsonSerializer.Serialize(product.ToProductResponse());
        await _cache.SetStringAsync(cacheKey, productJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)));
        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<PagedResult<ProductResponse>> GetProductsPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        IEnumerable<Product> items = await _productRepo.GetProductsPageProjectedAsync(page, pageSize, cancellationToken);

        int total = await _productRepo.GetActiveProductsCountAsync(cancellationToken);

        return new PagedResult<ProductResponse>
        {
            Items = items.Select(item => item.ToProductResponse()),
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public async Task<Result> UpdateProductAsync(ProductUpdateRequest product, Guid id, CancellationToken cancellationToken)
    {
        if (product == null)
        {
            return Result.Failure("The request body is required", StatusCodeEnum.BadRequest);
        }

        bool isModified = await _productRepo.UpdateProductAsync(product.ToProduct(), id, cancellationToken);

        if (!isModified)
        {
            return Result.Failure("Error: Product not found", StatusCodeEnum.NotFound);
        }

        string cacheKey = $"product:{id}";
        string cacheKey2 = "products:all";
        await _cache.RemoveAsync(cacheKey);
        await _cache.RemoveAsync(cacheKey2);

        return Result.Success("Product successfully updated!");
    }
}
