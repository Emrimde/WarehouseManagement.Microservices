using ProductService.Core.DTO;
using ProductService.Core.Result;

namespace ProductService.Core.ServiceContracts;
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetProducts();
    Task<Result<ProductResponse>> GetProductById(Guid id);
}
