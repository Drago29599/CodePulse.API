using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        //Defination of interface
        Task<BlogPost> AddBlogPostAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetAllPostAsunc();

        Task<BlogPost?> GetBlogByIdAsync(Guid Id);

        Task<BlogPost?> UpdateBlogPostById(BlogPost blogPost);

        Task<BlogPost?> DeleteBlogPostAsync(Guid Id);
    }
}
