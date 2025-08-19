using PickingMicroservice.Core.Domain.Entities;
using PickingMicroservice.Core.DTO;

namespace PickingMicroservice.Core.Mappers;
public static class PickingTaskMappers
{
    public static PickingResponse ToPickingResponse(this PickingTask pickingTask)
    {
        return new PickingResponse()
        {
            CreatedAt = DateTime.UtcNow,
            Id = pickingTask.Id,
            OrderId = pickingTask.OrderId,
            Quantity = pickingTask.Quantity,
            SKU = pickingTask.SKU,
            Status = pickingTask.Status,
            UpdatedAt = pickingTask.UpdatedAt
        };
    }
}
