using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Result;
using PickingMicroservice.Core.Service;

namespace PickingMicroservice.Core.ServiceContracts;

public interface IPickingService
{
    Task<IEnumerable<PickTaskResponse>> GetAllTasks();
    Task<Result<IEnumerable<PickItemResponse>>> GetTaskByOrderIdAsync(Guid pickTaskId);
    //Task<Result<bool>> MakeTaskCompleted(string orderId);
}