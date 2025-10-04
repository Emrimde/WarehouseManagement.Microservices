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

    public async Task<InventoryItem> initializeInventoryItemForProduct(string sku, string productName)
    {
        InventoryItem inventoryItem = new InventoryItem
        {
            Id = Guid.NewGuid(),                        
            StockKeepingUnit = sku,  
            ProductName = productName,
            QuantityOnHand = 0,                         
            QuantityReserved = 0,                       
            UpdatedAt = DateTime.UtcNow,
            UnitPrice = 0
        };

        _dbContext.InventoryItems.Add(inventoryItem); 
        await _dbContext.SaveChangesAsync();

        return inventoryItem;
    }

    public async Task<InventoryItem?> AdjustInventoryItem(string sku, InventoryItem inventoryItem,CancellationToken cancellationToken)
    {
        InventoryItem? existingInventoryItem = await GetInventoryBySkuAsync(sku,cancellationToken);
        if (existingInventoryItem == null) 
        {
            return null;
        }

        existingInventoryItem.QuantityOnHand += inventoryItem.QuantityOnHand;
        existingInventoryItem.UnitPrice = inventoryItem.UnitPrice;
        await _dbContext.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task<InventoryItem?> GetInventoryBySkuAsync(string sku,CancellationToken cancellationToken)
    {
        return await _dbContext.InventoryItems.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
    }

    public async Task<InventoryItem?> ReleaseQuantity(string sku, int adjustment)
    {
        InventoryItem? inventoryItem = await _dbContext.InventoryItems.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
        if (inventoryItem == null)
        {
            return null;
        }
        inventoryItem.QuantityReserved -= adjustment;
        await _dbContext.SaveChangesAsync();

        return inventoryItem;
    }

    public async Task<InventoryItem?> ReserveQuantity(string sku, int adjustment)
    {
        InventoryItem? inventoryItem = await _dbContext.InventoryItems.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);

        if (inventoryItem == null)
        {
            return null;
        }

        inventoryItem.QuantityReserved += adjustment;
        await _dbContext.SaveChangesAsync();

        return inventoryItem;
    }

    public async Task<IEnumerable<InventoryItem>> GetAllInventoryItems(int page, int pageSize, CancellationToken cancellationToken)
    {
        int offset = (page - 1) * pageSize;
        return await _dbContext.InventoryItems.AsNoTracking().OrderBy(item => item.QuantityOnHand).Skip(offset).Take(pageSize).ToListAsync(cancellationToken);
    }
    public async Task<int> GetInventoryItemsCount()
    {
        return await _dbContext.InventoryItems.CountAsync();
    }
}
