using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.DTO;

namespace InventoryMicroservice.Core.Mappers;
public static class InventoryItemMapper
{
    public static InventoryItemResponse ToInventoryItemResponse(this InventoryItem inventoryItem)
    {
        return new InventoryItemResponse
        (
            inventoryItem.QuantityOnHand - inventoryItem.QuantityReserved,
            inventoryItem.QuantityReserved,
            inventoryItem.QuantityOnHand,
            inventoryItem.UnitPrice
        );
    }

    public static InventoryItem ToInventoryItem(this InventoryUpdateRequest inventoryUpdateRequest)
    {
        return new InventoryItem()
        {
            UnitPrice = inventoryUpdateRequest.UnitPrice,
            QuantityOnHand = inventoryUpdateRequest.QuantityOnHand,
        };
    }
}
