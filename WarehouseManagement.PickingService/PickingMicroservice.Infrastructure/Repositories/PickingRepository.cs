using Microsoft.EntityFrameworkCore;
using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.Enum;
using PickingMicroservice.Infrastructure.DatabaseContext;

namespace PickingMicroservice.Infrastructure.Repositories;
public class PickingRepository : IPickingRepository
{
    private readonly ApplicationDbContext _dbContext;
    public PickingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<PickingTask>> GetAllTasks()
    {
        return await _dbContext.PickingTasks.Where(item => item.Status == PickStatus.InProgress).ToListAsync();
    }

    public async Task<PickingTask?> GetTaskById(string orderId)
    {
        return await _dbContext.PickingTasks.FirstOrDefaultAsync(item => item.OrderId == orderId);
    }

    public async Task<bool> MakeTaskCompleted(string orderId)
    {
        PickingTask? pickingTask = await GetTaskById(orderId);
        if (pickingTask == null)
        {
            return false;
        }
        pickingTask.Status = PickStatus.Completed;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
