using OrderMicroservice.Core.DTO;
using OrderMicroservice.Core.Result;

namespace OrderMicroservice.Core.ServiceContracts;
public interface IOrderService
{
    public Task<Result<bool>> AddOrder(OrderAddRequest request); 
}
