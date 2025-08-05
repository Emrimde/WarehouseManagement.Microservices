using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.DTO;

namespace InventoryMicroservice.Core.Mappers;
public static class InventoryItemMapper
{
    public static InventoryItemResponse ToInventoryItemResponse(this InventoryItem inventoryItem)
    {
        return new InventoryItemResponse()
        {
            Available = inventoryItem.QuantityOnHand - inventoryItem.QuantityReserved,
            QuantityReserved = inventoryItem.QuantityReserved,
            QuantityOnHand = inventoryItem.QuantityOnHand,  
        };
    }
}
