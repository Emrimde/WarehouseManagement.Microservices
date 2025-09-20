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

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _dbcontext.Categories.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<Category?> AddCategoryAsync(Category category)
        {
            await _dbcontext.Categories.AddAsync(category);
            await _dbcontext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            Category? category = await GetCategoryByIdAsync(id);
            if (category == null)
            {
                return false;
            }
            _dbcontext.Remove(category);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            Category? existingCategory = await GetCategoryByIdAsync(category.Id);

            if (existingCategory == null)
            {
                return false;
            }

            existingCategory.Name = category.Name;
            await _dbcontext.SaveChangesAsync();

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

        public async Task<int> GetActiveCategoriesAsync(CancellationToken cancellationToken)
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
    }
}
