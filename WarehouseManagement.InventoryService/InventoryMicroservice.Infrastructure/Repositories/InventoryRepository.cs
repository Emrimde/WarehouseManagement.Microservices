using InventoryMicroservice.Core.Domain.Entities;
using InventoryMicroservice.Core.Domain.RepositoryContracts;
using InventoryMicroservice.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace InventoryMicroservice.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _dbContext; 
        public InventoryRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<InventoryItem?> GetInventoryBySku(string sku)
        {
            return await _dbContext.InventoryItems.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
        }
    }
}
