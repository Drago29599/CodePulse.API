using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return category;
        }

        

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return  await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> UpdateCategoryByIdAsync(Category category)
        {
            var exsistingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (exsistingCategory != null) { 
                context.Entry(exsistingCategory).CurrentValues.SetValues(category);
                await context.SaveChangesAsync();
                return category;
            }
            return null;

        }

        public async Task<Category?> DeleteCategoryAsync(Guid id)
        {
            var exsistingCategory = await context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (exsistingCategory == null) {
                return null;
            }

            context.Categories.Remove(exsistingCategory);
            await context.SaveChangesAsync();
            return exsistingCategory;
        }
    }
}
