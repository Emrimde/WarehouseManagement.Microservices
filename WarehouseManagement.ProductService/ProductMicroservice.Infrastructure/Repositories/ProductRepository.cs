using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Core.RepositoryContracts;
using ProductMicroservice.Infrastructure.DatabaseContext;

namespace ProductMicroservice.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        product.IsActive = true;
        product.CreatedAt = DateTime.UtcNow; 
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        Product? product = await GetProductByIdAsync(id);
        if (product == null) {
            return false;
        }
        product.IsActive = false;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<int> GetActiveProductsCountAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Products.Where(item => item.IsActive).CountAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _dbContext.Products.Include(item => item.Category).FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<Product?> GetProductBySkuAsync(string sku)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.Include(item => item.Category).AsNoTracking().Where(item => item.IsActive == true).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsPageProjectedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        int offset = (page - 1) * pageSize;
        return await _dbContext.Products
            .Include(item => item.Category)
            .AsNoTracking()
            .Where(product => product.IsActive)
            .OrderBy(item => item.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsProductValid(Product product)
    {
        Category? category = await _dbContext.Categories.FindAsync(product.CategoryId);
        if (await _dbContext.Products.AnyAsync(item => item.Name == product.Name || item.Description == product.Description || item.StockKeepingUnit == product.StockKeepingUnit) || category == null){
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateProductAsync(Product product, Guid id)
    {
        Product? existingProduct = await GetProductByIdAsync(id);

        if (existingProduct == null)
        {
            return false;
        }

        existingProduct.Description = product.Description;
        existingProduct.Name = product.Name;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return true; 
    }
}
