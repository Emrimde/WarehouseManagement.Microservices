using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProductService.Core.Domain.Entities;
using ProductService.Core.DTO;
using ProductService.Core.Mappers;
using ProductService.Core.RabbitMQ;
using ProductService.Core.RepositoryContracts;
using ProductService.Core.Result;
using ProductService.Core.ServiceContracts;

namespace ProductService.Core.Services;
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

    public async Task<Result<ProductResponse>> DeleteProduct(Guid id)
    {
        if(id == Guid.Empty)
        {
            return Result<ProductResponse>.Failure("Invalid id", StatusCode.BadRequest);
        }

        bool isDeleted = await _productRepo.DeleteProduct(id);

        if (!isDeleted) 
        {
            return Result<ProductResponse>.Failure("The product don't exist", StatusCode.NotFound);
        }

        await _cache.RemoveAsync("products:all");
        await _cache.RemoveAsync($"products:{id}");

        return Result<ProductResponse>.SuccessResult("Deleted succesfully!");
    }

    public async Task<Result<ProductResponse>> GetProductByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Result<ProductResponse>.Failure("Incorrect id", StatusCode.BadRequest);
        }

        string cacheKey = $"product:{id}";
        string? cachedProduct = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedProduct))
        {
            ProductResponse? productFromCache = JsonConvert.DeserializeObject<ProductResponse>(cachedProduct);
            return Result<ProductResponse>.Success(productFromCache!);
        }

        Product? product = await _productRepo.GetProductByIdAsync(id);
        if (product == null)
        {
            return Result<ProductResponse>.Failure("Product not found", StatusCode.NotFound);
        }

        string productJson = JsonConvert.SerializeObject(product.ToProductResponse());
        await _cache.SetStringAsync(cacheKey, productJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100))); 

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
            ProductResponse? productFromCache = JsonConvert.DeserializeObject<ProductResponse>(cachedProduct);
            return Result<ProductResponse>.Success(productFromCache!);
        }

        Product? product = await _productRepo.GetProductBySkuAsync(sku);
        if (product == null)
        {
            return Result<ProductResponse>.Failure("Product not found", StatusCode.NotFound);
        }

        string productJson = JsonConvert.SerializeObject(product.ToProductResponse());
        await _cache.SetStringAsync(cacheKey, productJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)));
        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
    {
        string cacheKey = "products:all";
        string? cachedProducts = await _cache.GetStringAsync(cacheKey); 

        if (!string.IsNullOrEmpty(cachedProducts))
        {
            IEnumerable<ProductResponse>? productsFromCache = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(cachedProducts); 
            return productsFromCache!;
        }
        
        IEnumerable<Product> products = await _productRepo.GetProductsAsync(); 
        IEnumerable<ProductResponse> responseList = products.Select(item => item.ToProductResponse());

        string productsJson = JsonConvert.SerializeObject(responseList); 
        await _cache.SetStringAsync(cacheKey, productsJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)).SetSlidingExpiration(TimeSpan.FromSeconds(100)));

        return responseList;
    }

    public async Task<Result<ProductUpdateRequest>> UpdateProductAsync(ProductUpdateRequest product, Guid id)
    {
        if(product.Id != id)
        {
            return Result<ProductUpdateRequest>.Failure("Error: Id of the product is not equal to id", StatusCode.BadRequest);
        }

        bool isModified = await _productRepo.UpdateProductAsync(product.ToProduct(), id);

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
