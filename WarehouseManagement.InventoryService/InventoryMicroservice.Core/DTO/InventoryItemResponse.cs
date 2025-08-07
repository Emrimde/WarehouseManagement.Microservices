namespace InventoryMicroservice.Core.DTO;
public class InventoryItemResponse
{
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int Available { get; set; }
}
