using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Mappers;
using PickingMicroservice.Core.Result;
using PickingMicroservice.Core.ServiceContracts;
using PickingMicroservice.Infrastructure.Repositories;

namespace PickingMicroservice.Core.Service;
public class PickingService : IPickingService
{
    private readonly IPickingRepository _pickingRepo;
    private readonly IDistributedCache _cache;
    public PickingService(IPickingRepository pickingRepo, IDistributedCache cache)
    {
        _pickingRepo = pickingRepo;
        _cache = cache;
    }

    /// <summary>
    /// Get all pick tasks with in progress status
    /// </summary>
    /// <returns>List of pick tasks</returns>
    public async Task<IEnumerable<PickTaskResponse>> GetAllTasks()
    {
        string cacheKey = "pickTasks:all";
        string? cacheResponse = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            IEnumerable<PickTaskResponse>? pickTasksFromCache = JsonConvert.DeserializeObject<IEnumerable<PickTaskResponse>>(cacheResponse);
            return pickTasksFromCache!;
        }

        IEnumerable<PickTask> pickingTaskList = await _pickingRepo.GetAllTasksAsync();

        string pickTaskListInJson = JsonConvert.SerializeObject(pickingTaskList);

        await _cache.SetStringAsync(cacheKey, pickTaskListInJson, new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2.5)));

        return pickingTaskList.Select(item => item.ToPickingResponse());
    }

    /// <summary>
    /// Get all pick items to gather in magazine
    /// </summary>
    /// <param name="pickTaskId"></param>
    /// <returns></returns>
    public async Task<Result<IEnumerable<PickItemResponse>>> GetTaskByOrderIdAsync(Guid pickTaskId)
    {

        string cacheKey = $"pickItems:{pickTaskId}";
        string? cacheResponse = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            IEnumerable<PickItemResponse>? pickItemsFromCache = JsonConvert.DeserializeObject<IEnumerable<PickItemResponse>>(cacheResponse);

            return Result<IEnumerable<PickItemResponse>>.Success(pickItemsFromCache!);
        }

        if (pickTaskId == Guid.Empty)
        {
            return Result<IEnumerable<PickItemResponse>>.Failure("Identifier not correct", StatusCode.BadRequest);
        }
        IEnumerable<PickItem> pickingTask = await _pickingRepo.GetTaskByOrderIdAsync(pickTaskId);

        if (pickingTask == null)
        {
            return Result<IEnumerable<PickItemResponse>>.Failure("PickingTask not found", StatusCode.NotFound);
        }

        string pickItemsInJson = JsonConvert.SerializeObject(pickingTask);
        await _cache.SetStringAsync(cacheKey,pickItemsInJson,new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2.5)));

        return Result<IEnumerable<PickItemResponse>>.Success(pickingTask.Select(item => item.ToPickItemResponse()));
    }

    //public async Task<Result<bool>> MakeTaskCompleted(string orderId)
    //{
    //    if (string.IsNullOrEmpty(orderId))
    //    {
    //        return Result<bool>.Failure("Order is null or empty", StatusCode.BadRequest);
    //    }
    //    bool isDone = await _pickingRepo.MakeTaskCompleted(orderId);
    //    if (!isDone == false)
    //    {
    //        return Result<bool>.Failure("Picking status doesnt't change", StatusCode.NotFound);
    //    }
    //    return Result<bool>.Success(isDone);
    //}
}
