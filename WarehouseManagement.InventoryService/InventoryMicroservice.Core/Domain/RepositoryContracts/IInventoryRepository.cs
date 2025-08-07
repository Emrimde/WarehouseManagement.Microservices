using InventoryMicroservice.Core.Domain.Entities;

namespace InventoryMicroservice.Core.Domain.RepositoryContracts;
public interface IInventoryRepository
{
    Task<InventoryItem?> GetInventoryBySku(string sku);

    Task<InventoryItem> AddEmptyInventoryItemWithSkuAsync(string sku);
}
