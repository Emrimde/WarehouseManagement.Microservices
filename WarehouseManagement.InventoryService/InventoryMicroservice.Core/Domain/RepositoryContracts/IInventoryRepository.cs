using InventoryMicroservice.Core.Domain.Entities;

namespace InventoryMicroservice.Core.Domain.RepositoryContracts;
public interface IInventoryRepository
{
    Task<InventoryItem?> GetInventoryBySku(string sku);
    Task<InventoryItem> AddEmptyInventoryItemWithSkuAsync(string sku);
    Task<InventoryItem?> AdjustQuantity(string sku, int adjustment);
    Task<InventoryItem?> ReleaseQuantity(string sku, int adjustment);
    Task<InventoryItem?> ReserveQuantity(string sku, int adjustment);
}
