using Microsoft.EntityFrameworkCore;
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

    public async Task<bool> DeleteOrder(Guid id)
    {
        Order? order = await GetOrderById(id);
        if (order == null)
        {
            return false;
        }
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Order?> GetOrderById(Guid id)
    {
        Order? order = await _dbContext.Orders.FirstOrDefaultAsync(item => item.Id == id);
        if (order == null)
        {
            return null;
        }
        return order;
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<string?> GetOrderStatusById(Guid id)
    {
        Order? order = await GetOrderById(id);
        if (order == null)
        {
            return null;
        }
        return order.Status.ToString();
    }
}
