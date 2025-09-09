using Microsoft.EntityFrameworkCore;
using ProductService.Core.Domain.Entities;
using ProductService.Infrastructure.DatabaseContext;

namespace ProductMicroservice.Infrastructure.Repositories
{
    public class CategoryRepository
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
            if(category == null)
            {
                return false;
            }
            _dbcontext.Remove(category);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategory(Guid id, Category category)
        {
            Category? existingCategory = await GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                return false;
            }

            existingCategory.Name = category.Name;
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}
