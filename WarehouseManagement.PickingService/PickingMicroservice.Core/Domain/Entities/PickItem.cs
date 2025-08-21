namespace PickingMicroservice.Core.Domain.Entities;

public class PickItem
{
    public Guid Id { get; set; }
    public Guid PickTaskId { get; set; }
    public PickTask PickTask { get; set; } = default!;

    public string SKU { get; set; } = null!;
    public int Quantity { get; set; }
    //public int PickedQty { get; set; } = 0;
    //public string? Location { get; set; } 
    //public

}
