using InventoryMicroservice.Core.DTO;
using InventoryMicroservice.Core.Result;

namespace InventoryMicroservice.Core.ServiceContracts;
public interface IInventoryService
{
    Task<Result<InventoryItemResponse>> GetInventoryBySkuAsync(string sku, CancellationToken cancellationToken);
    Task<Result<InventoryItemResponse>> AdjustInventoryItem(string sku, InventoryUpdateRequest request,CancellationToken cancellationToken);
    Task<PagedResult<InventoryItemResponse>> GetAllInventories(int page, int pageSize, CancellationToken cancellationToken);
}
