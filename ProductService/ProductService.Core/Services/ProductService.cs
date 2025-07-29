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

        Product? product = await _productRepo.GetProductByIdAsync(id);
        if (product == null)
        {
            return Result<ProductResponse>.Failure("Product not found", StatusCode.NotFound);
        }

        return Result<ProductResponse>.Success(product.ToProductResponse());
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
    {
        IEnumerable<Product> products = await _productRepo.GetProductsAsync();
        return products.Select(item => item.ToProductResponse());
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
