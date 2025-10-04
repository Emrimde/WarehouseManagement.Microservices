using InventoryMicroservice.Core.Domain.Entities;

namespace InventoryMicroservice.Core.Domain.RepositoryContracts;
public interface IInventoryRepository
{
    Task<InventoryItem?> GetInventoryBySkuAsync(string sku, CancellationToken cancellationToken);
    Task<InventoryItem> initializeInventoryItemForProduct(string sku, string productName);
    Task<InventoryItem?> AdjustInventoryItem(string sku, InventoryItem inventoryItem, CancellationToken cancellationToken);
    Task<InventoryItem?> ReserveQuantity(string sku, int adjustment);
    Task<IEnumerable<InventoryItem>> GetAllInventoryItems(int page, int pageSize, CancellationToken cancellationToken);
    Task<InventoryItem?> ReleaseQuantity(string sku, int adjustment);
    Task<int> GetInventoryItemsCount();
}
