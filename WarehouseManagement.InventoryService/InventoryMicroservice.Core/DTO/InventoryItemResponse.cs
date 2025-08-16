namespace InventoryMicroservice.Core.DTO;
public record InventoryItemResponse
(
     int QuantityOnHand,
     int QuantityReserved, 
     int Available,
     decimal UnitPrice 
);
