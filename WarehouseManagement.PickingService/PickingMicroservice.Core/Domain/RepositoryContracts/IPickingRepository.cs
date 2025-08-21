using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.RabbitMQ.InventoryConsumer;

namespace PickingMicroservice.Infrastructure.Repositories;
public interface IPickingRepository
{
    Task<IEnumerable<PickTask>> GetAllTasksAsync();
    Task<IEnumerable<PickItem>> GetTaskByOrderIdAsync(Guid pickTaskId);
    Task<PickTask> CreatePickTask(PickingMessage message);
    //Task<bool> MakeTaskCompleted(string orderId);
}