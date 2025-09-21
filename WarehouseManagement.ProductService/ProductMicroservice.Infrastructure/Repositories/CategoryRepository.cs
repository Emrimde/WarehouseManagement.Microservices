using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Core.Domain.Entities;
using ProductMicroservice.Infrastructure.DatabaseContext;

namespace ProductMicroservice.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public CategoryRepository(ApplicationDbContext context)
        {
            _dbcontext = context;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbcontext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(Guid id,CancellationToken cancellationToken)
        {
            return await _dbcontext.Categories.FindAsync(id,cancellationToken);
        }

        public async Task<Category?> AddCategoryAsync(Category category)
        {
            await _dbcontext.Categories.AddAsync(category);
            await _dbcontext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            Category? category = await _dbcontext.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _dbcontext.Remove(category);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryNameAsync(Guid id, string name, CancellationToken cancellationToken)
        {
            Category? existingCategory = await _dbcontext.Categories.FindAsync(id,cancellationToken);

            if (existingCategory == null)
            {
                return false;
            }

            existingCategory.Name = name;
            await _dbcontext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<Product>> GetRelatedProductsWithCategoryId(Guid id)
        {
            Category? category = await _dbcontext.Categories
                                           .AsNoTracking()
                                           .Include(c => c.Products)
                                           .FirstOrDefaultAsync(c => c.Id == id);

            return category?.Products ?? Enumerable.Empty<Product>();
        }

        public async Task<int> GetCategoriesCountAsync(CancellationToken cancellationToken)
        {
            return await _dbcontext.Categories.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            int offset = (page - 1) * pageSize;
            return await _dbcontext.Categories
                 .AsNoTracking()
                 .OrderBy(item => item.Id)
                 .Skip(offset)
                 .Take(pageSize)
                 .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsCategoryNameUnique(string name, CancellationToken cancellationToken)
        {
            return await _dbcontext.Categories.AnyAsync(item => item.Name == name, cancellationToken);
        }
    }
}
