using OrderMicroservice.Core.DTO.OrderDTO;
using OrderMicroservice.Core.Result;

namespace OrderMicroservice.Core.ServiceContracts;
public interface IOrderService
{
    Task<Result<OrderResponse>> AddOrder(OrderAddRequest request);
    Task<IEnumerable<OrderResponse>> GetAllOrders();
    Task<Result<OrderResponse>> GetOrderById(Guid id);
    Task<Result<string>> GetOrderStatusById(Guid id);
    Task<Result<bool>> DeleteOrder(Guid id);
}
