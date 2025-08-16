namespace OrderMicroservice.Core.DTO.InventoryDTO;
public record InventoryItemResponse
(
     int QuantityOnHand,
     int QuantityReserved,
     int Available,
     decimal UnitPrice
);
