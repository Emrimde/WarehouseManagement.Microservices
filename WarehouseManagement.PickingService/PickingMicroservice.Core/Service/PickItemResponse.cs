namespace PickingMicroservice.Core.Service;
public class PickItemResponse
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = null!;
    public int Quantity { get; set; }
}