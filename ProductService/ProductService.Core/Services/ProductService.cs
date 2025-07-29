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
    public ProductService(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<ProductResponse>> GetProductById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Result<ProductResponse>.Failure("Incorrect id", StatusCode.BadRequest);
        }

        Product? product = await _productRepo.GetProductById(id);
        if (product == null)
        {
            return Result<ProductResponse>.Failure("Product not found", StatusCode.NotFound);
        }

        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<IEnumerable<ProductResponse>> GetProducts()
    {
        IEnumerable<Product> products = await _productRepo.GetProducts();
        return products.Select(item => item.ToProductResponse());
    }
}
