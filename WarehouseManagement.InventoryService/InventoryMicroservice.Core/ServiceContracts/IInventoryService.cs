using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Result;

namespace InventoryMicroservice.Core.ServiceContracts;
public interface IInventoryService
{
    Task<Result<InventoryItemResponse>> GetInventoryBySku(string sku);
    Task<Result<InventoryItemResponse>> AdjustInventoryItem(string sku, InventoryUpdateRequest request);
}
