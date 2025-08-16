using OrderMicroservice.Core.Domain.Entities;

namespace OrderMicroservice.Core.Domain.RepositoryContracts;
public interface IOrderRepository
{
    public Task<Order> AddOrder(Order order);
}
