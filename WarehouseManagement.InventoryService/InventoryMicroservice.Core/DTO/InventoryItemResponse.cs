namespace InventoryMicroservice.Core.DTO;
public record InventoryItemResponse
(
     string stockKeepingUnit,
     string productName,
     int QuantityOnHand,
     int QuantityReserved, 
     int Available,
     decimal? UnitPrice 
);
