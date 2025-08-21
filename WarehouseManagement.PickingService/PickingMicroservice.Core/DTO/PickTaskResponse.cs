using PickingMicroservice.Core.Enum;

namespace PickingMicroservice.Core.DTO;
public class PickTaskResponse
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = default!;
    public string OrderNumber { get; set; } = default!;
    public PickTaskStatus Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;
}