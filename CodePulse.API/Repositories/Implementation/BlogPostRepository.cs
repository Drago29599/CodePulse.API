using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext dbContext;

        public BlogPostRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> AddBlogPostAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        

        public async Task<IEnumerable<BlogPost>> GetAllPostAsunc()
        {
            return await dbContext.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogByIdAsync(Guid Id)
        {
            return await dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == Id);

        }

        public async Task<BlogPost> UpdateBlogPostById(BlogPost blogPost)
        {
            var exsistingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (exsistingBlogPost != null) {
                dbContext.Entry(exsistingBlogPost).CurrentValues.SetValues(blogPost);

                exsistingBlogPost.Categories = blogPost.Categories; 

                await dbContext.SaveChangesAsync();
                return blogPost;
            }

            return null;
        }

        public async Task<BlogPost?> DeleteBlogPostAsync(Guid Id)
        {
           var exsistingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == Id);
            if(exsistingBlogPost == null)
            {
                return null;
            }
             dbContext.BlogPosts.Remove(exsistingBlogPost);
            await dbContext.SaveChangesAsync();

            return exsistingBlogPost;

        }
    }
}
