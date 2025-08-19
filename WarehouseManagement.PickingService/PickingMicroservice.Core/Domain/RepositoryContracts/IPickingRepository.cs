using PickingMicroservice.Core.Domain.Entities;

namespace PickingMicroservice.Infrastructure.Repositories;
public interface IPickingRepository
{
    Task<IEnumerable<PickingTask>> GetAllTasks();
    Task<PickingTask?> GetTaskById(string orderId);
    Task<bool> MakeTaskCompleted(string orderId);
}