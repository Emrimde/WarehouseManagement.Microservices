using PickingMicroservice.Core.Enum;

namespace PickingMicroservice.Core.Domain.Entities;
public class PickTask
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = default!;
    public string? OrderNumber { get; set; } = default!;
    public string? CustomerName { get; set; } = default!;
    //public string? ShippingAddress { get; set; } = default!;
    public PickTaskStatus Status { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    //public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<PickItem> Items { get; set; } = new List<PickItem>();
}
