namespace InventoryMicroservice.Core.DTO;

public record InventoryUpdateRequest
(
 int QuantityOnHand,
 decimal? UnitPrice
);
