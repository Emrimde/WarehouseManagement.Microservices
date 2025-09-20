using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.Domain.RepositoryContracts;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Infrastructure.DatabaseContext;

namespace ProductMicroservice.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        product.IsActive = true;
        product.CreatedAt = DateTime.UtcNow; 
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        Product? product = await _dbContext.Products.FindAsync(id, cancellationToken);
        if (product == null) {
            return false;
        }

        if (!product.IsActive)
        {
            return true;
        }

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> GetActiveProductsCountAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Products.Where(item => item.IsActive).CountAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id,CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id && item.IsActive == false);
    }

    public async Task<Product?> GetProductBySkuAsync(string sku)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
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
    public async Task<bool> IsCategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.AnyAsync(item => item.Id == categoryId, cancellationToken);
    }
    public async Task<bool> IsSkuExistsAsync(string sku, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return false;
        }
        return await _dbContext.Products.AnyAsync(item => item.StockKeepingUnit == sku, cancellationToken);
    }

    public async Task<bool> ExistsByNameInCategoryAsync(string name, Guid categoryId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        string normName = name.Trim();
        return await _dbContext.Products.AnyAsync(item => item.CategoryId == categoryId && item.Name == normName, cancellationToken);
    }
    public async Task<bool> UpdateProductAsync(Product product, Guid id, CancellationToken cancellationToken)
    {
        Product? existingProduct = await _dbContext.Products
    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (existingProduct == null)
        {
            return false;
        }

        existingProduct.Description = product.Description;
        existingProduct.Name = product.Name;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true; 
    }
}
