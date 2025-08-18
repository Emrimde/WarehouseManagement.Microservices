using OrderMicroservice.Core.Domain.Entities;

namespace OrderMicroservice.Core.Domain.RepositoryContracts;
public interface IOrderRepository
{
    Task<Order> AddOrder(Order order);
    Task<IEnumerable<Order>> GetAllOrders();
    Task<Order?> GetOrderById(Guid id);
    Task<string?> GetOrderStatusById(Guid id);
    Task<bool> DeleteOrder(Guid id);
}
