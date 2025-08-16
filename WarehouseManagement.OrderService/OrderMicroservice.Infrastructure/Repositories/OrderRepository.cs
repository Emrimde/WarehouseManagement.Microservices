using OrderMicroservice.Core.Domain.Entities;
using OrderMicroservice.Core.Domain.RepositoryContracts;
using OrderMicroservice.Core.Enums;
using OrderMicroservice.Infrastructure.DatabaseContext;

namespace OrderMicroservice.Infrastructure.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;
    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> AddOrder(Order order)
    {
        order.OrderNumber = Guid.NewGuid().ToString().ToUpper();
        order.CreatedAt = DateTime.UtcNow;
        order.Status = OrderStatus.Pending;
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }
}
