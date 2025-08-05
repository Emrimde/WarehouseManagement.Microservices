namespace InventoryMicroservice.Core.Domain.Entities;

public class InventoryItem
{
    public Guid Id { get; set; }
    public string StockKeepingUnit { get; set; } = default!;  
    public int QuantityOnHand { get; set; }                    
    public int QuantityReserved { get; set; }                  
    public DateTime UpdatedAt { get; set; }
}
