using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Service;

namespace PickingMicroservice.Core.Mappers;
public static class PickingTaskMappers
{
    public static PickTaskResponse ToPickingResponse(this PickTask pickingTask)
    {
        return new PickTaskResponse()
        {
            CreatedAt = DateTime.UtcNow,
            Id = pickingTask.Id,
            OrderId = pickingTask.OrderId,
            Status = pickingTask.Status,
            OrderNumber = pickingTask.OrderNumber
        };
    }
    public static PickItemResponse ToPickItemResponse(this PickItem pickItem)
    {
        return new PickItemResponse()
        {
            Id = pickItem.Id,
            Quantity = pickItem.Quantity,
            SKU = pickItem.SKU,
        };
    }
}
