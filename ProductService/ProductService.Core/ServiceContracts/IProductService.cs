using ProductService.Core.DTO;

namespace ProductService.Core.ServiceContracts;
public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetProducts();
}
