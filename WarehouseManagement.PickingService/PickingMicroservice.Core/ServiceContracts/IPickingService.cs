using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Result;

namespace PickingMicroservice.Core.ServiceContracts
{
    public interface IPickingService
    {
        Task<IEnumerable<PickingResponse>> GetAllTasks();
        Task<Result<PickingResponse>> GetTaskById(string orderId);
        Task<Result<bool>> MakeTaskCompleted(string orderId);
    }
}