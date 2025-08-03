using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProductService.Core.Domain.Entities;
using ProductService.Core.DTO;
using ProductService.Core.Mappers;
using ProductService.Core.RepositoryContracts;
using ProductService.Core.Result;
using ProductService.Core.ServiceContracts;

namespace ProductService.Core.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly IDistributedCache _cache;
    public ProductService(IProductRepository productRepo, IDistributedCache cache)
    {
        _productRepo = productRepo;
        _cache = cache;
    }
    public async Task<Result<ProductResponse>> AddProductAsync(ProductAddRequest product)
    {
        if(!await _productRepo.IsProductValid(product.ToProduct()))
        {
            return Result<ProductResponse>.Failure("Product is not unique. Change data to create new product", StatusCode.Conflict);
        }

        Product newProduct = await _productRepo.AddProductAsync(product.ToProduct());

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
        await _cache.SetStringAsync(cacheKey, productJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(40)).SetSlidingExpiration(TimeSpan.FromSeconds(20))); // Jeżeli cache nie jest uzywany po 20 sekundach znika ale po 40 sekundach czy było używane czy nie, cache sie usuwa
        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
    {
        //MECHANIZM CZYTANIA Z CACHE
        string cacheKey = "products:all";  // tworzenie klucza cache
        string? cachedProducts = await _cache.GetStringAsync(cacheKey); // próba uzyskania danych z cache dla stworzonego klucza (przy pierwszym podejsciu bedzie null

        if (!string.IsNullOrEmpty(cachedProducts)) // jesli są dane w cache
        {
            IEnumerable<ProductResponse>? productsFromCache = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(cachedProducts); // deserializuj (zamiana format json na obiekty c#)
            return productsFromCache!;
        }
        // Jeśli nie ma danych w cache idziemy dalej
        IEnumerable<Product> products = await _productRepo.GetProductsAsync(); // wyciagamy z bazki
        IEnumerable<ProductResponse> responseList = products.Select(item => item.ToProductResponse());

        // MECHANIZM ZAPISYWANIA DO CACHE
        string productsJson = JsonConvert.SerializeObject(responseList); // serializacja( zamiana obiektow c# na json, po to zeby schowac w cache)
        await _cache.SetStringAsync(cacheKey, productsJson, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60)).SetSlidingExpiration(TimeSpan.FromSeconds(30))); // na podany klucz(cacheKey) zapisujemy dane produktu(productsJson) na tak długo (Set - Absolute i Sliding)

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

        return Result<ProductUpdateRequest>.SuccessResult("Product successfully updated!");
    }
}
