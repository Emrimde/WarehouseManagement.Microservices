using Microsoft.Extensions.Caching.Distributed;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.Domain.RepositoryContracts;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Core.RabbitMQ;
using ProductMicroservice.Core.Result;
using ProductMicroservice.Core.ServiceContracts;
using System.Text.Json;


namespace ProductMicroservice.Core.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly IDistributedCache _cache;
    private readonly IRabbitMQPublisher _publisher;
    public ProductService(IProductRepository productRepo, IDistributedCache cache, IRabbitMQPublisher publisher)
    {
        _productRepo = productRepo;
        _cache = cache;
        _publisher = publisher;
    }
    public async Task<Result<ProductResponse>> AddProductAsync(ProductAddRequest product)
    {
        if(!await _productRepo.IsProductValid(product.ToProduct()))
        {
            return Result<ProductResponse>.Failure("Product is not unique. Change data to create new product", StatusCode.Conflict);
        }

        Product newProduct = await _productRepo.AddProductAsync(product.ToProduct());

         _publisher.Publish("product.create", new ProductCreateMessage(newProduct.StockKeepingUnit, newProduct.Name));

        await _cache.RemoveAsync("products:all");

        return Result<ProductResponse>.Success(newProduct.ToProductResponse());
    }

    public async Task<Result<ProductResponse>> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        if(id == Guid.Empty)
        {
            return Result<ProductResponse>.Failure("Invalid id", StatusCode.BadRequest);
        }

        bool isDeleted = await _productRepo.DeleteProduct(id, cancellationToken);

        if (!isDeleted) 
        {
            return Result<ProductResponse>.Failure("The product don't exist", StatusCode.NotFound);
        }

        await _cache.RemoveAsync("products:all");
        await _cache.RemoveAsync($"products:{id}");

        return Result<ProductResponse>.SuccessResult("Deleted succesfully!");
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
            return Result<ProductResponse>.Failure("Product not found", StatusCode.NotFound);
        }

        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(product.ToProductResponse());
        await _cache.SetAsync(cacheKey, bytes, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)), cancellationToken); 

        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<Result<ProductResponse>> GetProductBySkuAsync(string sku)
    {
        if (string.IsNullOrEmpty(sku))
        {
            return Result<ProductResponse>.Failure("Incorrect id", StatusCode.BadRequest);
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
            return Result<ProductResponse>.Failure("Product not found", StatusCode.NotFound);
        }

        string productJson = JsonSerializer.Serialize(product.ToProductResponse());
        await _cache.SetStringAsync(cacheKey, productJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)));
        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
    {
        string cacheKey = "products:all";
        string? cachedProducts = await _cache.GetStringAsync(cacheKey); 

        if (!string.IsNullOrEmpty(cachedProducts))
        {
            IEnumerable<ProductResponse>? productsFromCache = JsonSerializer.Deserialize<IEnumerable<ProductResponse>>(cachedProducts); 
            return productsFromCache!;
        }
        
        IEnumerable<Product> products = await _productRepo.GetProductsAsync(); 
        IEnumerable<ProductResponse> responseList = products.Select(item => item.ToProductResponse());

        string productsJson = JsonSerializer.Serialize(responseList); 
        await _cache.SetStringAsync(cacheKey, productsJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)));

        return responseList;
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

    public async Task<Result<ProductUpdateRequest>> UpdateProductAsync(ProductUpdateRequest product, Guid id, CancellationToken cancellationToken)
    {
        if(product.Id != id)
        {
            return Result<ProductUpdateRequest>.Failure("Error: Id of the product is not equal to id in query", StatusCode.BadRequest);
        }

        bool isModified = await _productRepo.UpdateProductAsync(product.ToProduct(), id, cancellationToken);

        if (!isModified)
        {
            return Result<ProductUpdateRequest>.Failure("Error: Product not found", StatusCode.NotFound);
        }

        string cacheKey = $"product:{id}";
        string cacheKey2 = "products:all";
        await _cache.RemoveAsync(cacheKey);
        await _cache.RemoveAsync(cacheKey2);

        return Result<ProductUpdateRequest>.SuccessResult("Product successfully updated!");
    }
}
