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
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _dbContext.Products.Where(item => item.IsActive == true).ToListAsync();
    }
}
