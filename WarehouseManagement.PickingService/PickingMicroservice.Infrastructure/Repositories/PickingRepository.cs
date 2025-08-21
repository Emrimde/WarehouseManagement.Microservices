using Microsoft.EntityFrameworkCore;
using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.Enum;
using PickingMicroservice.Core.RabbitMQ.InventoryConsumer;
using PickingMicroservice.Infrastructure.DatabaseContext;

namespace PickingMicroservice.Infrastructure.Repositories;
public class PickingRepository : IPickingRepository
{
    private readonly ApplicationDbContext _dbContext;
    public PickingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PickTask> CreatePickTask(PickingMessage message)
    {
        PickTask pickTask = new PickTask()
        {
            CreatedAt = message.CreatedAt,
            CustomerName = message.CustomerName,
            Items = message.Items.Select(item => new PickItem {
                Quantity = item.Quantity,
                SKU = item.SKU,
            }).ToList(),
            OrderNumber = message.OrderNumber,
            OrderId = message.orderId,
            Status = PickTaskStatus.InProgress,
            
        };
        _dbContext.PickTasks.Add(pickTask);
        await _dbContext.SaveChangesAsync();
        return pickTask;
    }

    public async Task<IEnumerable<PickTask>> GetAllTasksAsync()
    {
        return await _dbContext.PickTasks.Where(item => item.Status == PickTaskStatus.InProgress).ToListAsync();
    }

    public async Task<IEnumerable<PickItem>> GetTaskByOrderIdAsync(Guid pickTaskId)
    {
      return await _dbContext.PickItem.Where(item => item.PickTaskId == pickTaskId).ToListAsync();
    }

  

    //public async Task<bool> MakeTaskCompleted(string orderId)
    //{
    //    PickTask? pickingTask = await GetTaskById(orderId);
    //    if (pickingTask == null)
    //    {
    //        return false;
    //    }
    //    pickingTask.Status = PickTaskStatus.Completed;
    //    await _dbContext.SaveChangesAsync();
    //    return true;
    //}
}
