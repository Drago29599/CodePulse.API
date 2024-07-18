using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        //Collection of entities in Relation DB
        public DbSet<BlogPost> BlogPosts { get; set; }

        //Collection of entities in Relation DB
        public DbSet<Category> Categories { get; set; }
    }
}
