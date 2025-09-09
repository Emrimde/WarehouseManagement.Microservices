using InventoryMicroservice.Core.Domain.Entities;

namespace InventoryMicroservice.Core.Domain.RepositoryContracts;
public interface IInventoryRepository
{
    Task<InventoryItem?> GetInventoryBySku(string sku);
    Task<InventoryItem> initializeInventoryItemForProduct(string sku, string productName);
    Task<InventoryItem?> AdjustInventoryItem(string sku, InventoryItem inventoryItem);
    Task<InventoryItem?> ReleaseQuantity(string sku, int adjustment);
    Task<InventoryItem?> ReserveQuantity(string sku, int adjustment);
}
