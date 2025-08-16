using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace InventoryMicroservice.Infrastructure.Repositories;
public class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _dbContext; 
    public InventoryRepository(ApplicationDbContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public async Task<InventoryItem> AddEmptyInventoryItemWithSkuAsync(string sku)
    {
        InventoryItem inventoryItem = new InventoryItem
        {
            Id = Guid.NewGuid(),                        
            StockKeepingUnit = sku,            
            QuantityOnHand = 0,                         
            QuantityReserved = 0,                       
            UpdatedAt = DateTime.UtcNow               
        };

        _dbContext.InventoryItems.Add(inventoryItem); 
        await _dbContext.SaveChangesAsync();

        return inventoryItem;
    }

    public async Task<InventoryItem?> AdjustQuantity(string sku, int adjustment)
    {
        InventoryItem? inventoryItem = await GetInventoryBySku(sku);
        if (inventoryItem == null) 
        {
            return null;
        }
        inventoryItem.QuantityOnHand += adjustment;
        await _dbContext.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task<InventoryItem?> GetInventoryBySku(string sku)
    {
        return await _dbContext.InventoryItems.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
    }

    public async Task<InventoryItem?> ReleaseQuantity(string sku, int adjustment)
    {
        InventoryItem? inventoryItem = await GetInventoryBySku(sku);
        if(inventoryItem == null)
        {
            return null;
        }
        inventoryItem.QuantityReserved -= adjustment;
        await _dbContext.SaveChangesAsync();

        return inventoryItem;
    }

    public async Task<InventoryItem?> ReserveQuantity(string sku, int adjustment)
    {
        InventoryItem? inventoryItem = await GetInventoryBySku(sku);

        if (inventoryItem == null)
        {
            return null;
        }

        inventoryItem.QuantityReserved += adjustment;
        await _dbContext.SaveChangesAsync();

        return inventoryItem;
    }
}
