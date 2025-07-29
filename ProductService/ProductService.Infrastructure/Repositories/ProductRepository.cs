using Microsoft.EntityFrameworkCore;
using ProductService.Core.Domain.Entities;
using ProductService.Core.RepositoryContracts;
using ProductService.Infrastructure.DatabaseContext;

namespace ProductService.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.Where(item => item.IsActive == true).ToListAsync();
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
        existingProduct.UpdatedAt = DateTime.Now;

        return true; 
    }
}
