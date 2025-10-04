using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Core.Domain.RepositoryContracts;
using ProductMicroservice.Core.DTO;
using ProductMicroservice.Core.Enums;
using ProductMicroservice.Core.Mappers;
using ProductMicroservice.Infrastructure.DatabaseContext;

namespace ProductMicroservice.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProductRepository> _logger;
    public ProductRepository(ApplicationDbContext dbContext,ILogger<ProductRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
        if (product == null)
        {
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

    public async Task<int> GetProductsCountAsync(bool showActive, CancellationToken cancellationToken)
    {
        return await _dbContext.Products.Where(item => item.IsActive == showActive).CountAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id && item.IsActive, cancellationToken);
    }

    public async Task<Product?> GetProductBySkuAsync(string sku)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(item => item.StockKeepingUnit == sku);
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

        if (existingProduct.Name == product.Name && existingProduct.Description == product.Description)
        {
            return false;
        }

        existingProduct.Description = product.Description;
        existingProduct.Name = product.Name;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<Product>> GetProductsPageProjectedAsync(int page, int pageSize, string? name, ProductSearchCategoriesEnum category, CancellationToken cancellationToken, bool showActive)
    {
        int offset = (page - 1) * pageSize;
        IQueryable<Product> products = null!;

        if (showActive)
        {
            products = _dbContext.Products
               .Include(item => item.Category)
               .AsNoTracking()
               .Where(product => product.IsActive);
        }
        else
        {
            products = _dbContext.Products
               .Include(item => item.Category)
               .AsNoTracking()
               .Where(product => !product.IsActive);
        }

        if (category != ProductSearchCategoriesEnum.None && name != null)
        {
            switch (category)
            {
                case ProductSearchCategoriesEnum.Name:
                    products = products.Where(item => item.Name.Contains(name)); break;
                case ProductSearchCategoriesEnum.Manufacturer:
                    products = products.Where(item => item.Manufacturer.Contains(name)); break;
                case ProductSearchCategoriesEnum.Description:
                    products = products.Where(item => item.Description.Contains(name)); break;


            }
        }
        else if (category == ProductSearchCategoriesEnum.None && name != null)
        {
            products = products.Where(item => item.Name.Contains(name));
        }


        return await products.OrderBy(item => item.Id)
        .Skip(offset)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
    }

    public async Task<bool> PermanentDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        int isDeleted = await _dbContext.SaveChangesAsync();

        if (isDeleted == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RestoreProductAsync(Guid id, CancellationToken cancellationToken)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (product == null)
        {
            return false;
        }

        product.IsActive = true;
        int restoredRecord = 0;
        try
        {
            restoredRecord = await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning("Database update error {exception}", ex);
            return false;
        }
        return restoredRecord > 0;
    }
}
