using PickingMicroservice.Core.Enum;

namespace PickingMicroservice.Core.DTO;

public class PickingResponse
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public PickStatus Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;
    public DateTime UpdatedAt { get; set; } = default!;
}