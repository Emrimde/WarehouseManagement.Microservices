using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Enum;
using PickingMicroservice.Core.Mappers;
using PickingMicroservice.Core.Result;
using PickingMicroservice.Core.ServiceContracts;
using PickingMicroservice.Infrastructure.Repositories;

namespace PickingMicroservice.Core.Service;
public class PickingService : IPickingService
{
    private readonly IPickingRepository _pickingRepo;
    public PickingService(IPickingRepository pickingRepo)
    {
        _pickingRepo = pickingRepo;
    }

    public async Task<IEnumerable<PickingResponse>> GetAllTasks()
    {
        IEnumerable<PickingTask> pickingTaskList = await _pickingRepo.GetAllTasks();

        return pickingTaskList.Select(item => item.ToPickingResponse());
    }

    public async Task<Result<PickingResponse>> GetTaskById(string orderId)
    {
        if (string.IsNullOrEmpty(orderId))
        {
            return Result<PickingResponse>.Failure("Order is null or empty", StatusCode.BadRequest);
        }
        PickingTask? pickingTask = await _pickingRepo.GetTaskById(orderId);
        if (pickingTask == null)
        {
            return Result<PickingResponse>.Failure("PickingTask not found", StatusCode.NotFound);
        }
        return Result<PickingResponse>.Success(pickingTask.ToPickingResponse());
    }

    public async Task<Result<bool>> MakeTaskCompleted(string orderId)
    {
        if (string.IsNullOrEmpty(orderId))
        {
            return Result<bool>.Failure("Order is null or empty", StatusCode.BadRequest);
        }
        bool isDone = await _pickingRepo.MakeTaskCompleted(orderId);
        if (!isDone == false)
        {
            return Result<bool>.Failure("Picking status doesnt't change", StatusCode.NotFound);
        }
        return Result<bool>.Success(isDone);
    }
}
