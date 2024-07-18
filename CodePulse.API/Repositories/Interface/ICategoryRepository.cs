using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        //Defination in interface
        Task<Category> CreateAsync(Category category); 

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(Guid id);

        Task<Category?> UpdateCategoryByIdAsync(Category category);

        Task<Category?> DeleteCategoryAsync(Guid id);
    }
}
