using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddblogPost([FromBody] AddBlogPostReqDto model)
        {
            try
            {


                //Map dto to domain model
                var blogPost = new BlogPost
                {
                    Title = model.Title,
                    UrlHandle = model.UrlHandle,
                    ShortDescription = model.ShortDescription,
                    PublishedDate = model.PublishedDate,
                    Content = model.Content,
                    FeaturedIamgeUrl = model.FeaturedImageUrl,
                    IsVisible = model.IsVisible,
                    Author = model.Author,
                    Categories = new List<Category>()
                };

                foreach (var categoryGuid in model.Categories)
                {
                    var exsistingCategory = await categoryRepository.GetCategoryByIdAsync(categoryGuid);
                    if (exsistingCategory is not null) {
                        blogPost.Categories.Add(exsistingCategory);
                    }
                }

                //Repo calls
                blogPost = await blogPostRepository.AddBlogPostAsync(blogPost);
                //Domain to dto

                var response = new BlogPostDto()
                {
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedIamgeUrl,
                    IsVisible = blogPost.IsVisible,
                    Author = blogPost.Author,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                };

                return Ok(response);

            }
            catch (Exception)
            {

                throw;
            }

            return BadRequest("Something went wrong!!");

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPosts = await blogPostRepository.GetAllPostAsunc();

            var response = new List<BlogPostDto>();

            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id=blogPost.Id,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedIamgeUrl,
                    IsVisible = blogPost.IsVisible,
                    Author = blogPost.Author,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                });
            }

            return Ok(response);

        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetBlogPostById(Guid Id)
        {
            //Get blog post from repo
            var blogPost = await blogPostRepository.GetBlogByIdAsync(Id);

            if (blogPost == null)
            {
                return BadRequest();
            }
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedIamgeUrl,
                IsVisible = blogPost.IsVisible,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute]Guid Id,[FromBody]UpdateBlogpostReqDto model)
        {
            //Map req Dtp to domain model
            var blogPost = new BlogPost
            {
                Id = Id,
                Title = model.Title,
                UrlHandle = model.UrlHandle,
                ShortDescription = model.ShortDescription,
                PublishedDate = model.PublishedDate,
                Content = model.Content,
                FeaturedIamgeUrl = model.FeaturedImageUrl,
                IsVisible = model.IsVisible,
                Author = model.Author,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in model.Categories)
            {
                var exsistingCategory = await categoryRepository.GetCategoryByIdAsync(categoryGuid);
                if (exsistingCategory is not null)
                {
                    blogPost.Categories.Add(exsistingCategory);
                }
            }

            //Calling repo
            var updatedBlogpost = await blogPostRepository.UpdateBlogPostById(blogPost);

            if (updatedBlogpost == null)
            {
                return NotFound();
            }

            //Domain model to dto 
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedIamgeUrl,
                IsVisible = blogPost.IsVisible,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };


            //return value
            return Ok(response);
        }

        //DELETE
        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid Id)
        {
            //Call repo to delete
            var deletedBlogpost = await blogPostRepository.DeleteBlogPostAsync(Id);
            if (deletedBlogpost == null) { return NotFound(); }

            //convert domain model to dto
            var response = new BlogPostDto
            {
                Id = deletedBlogpost.Id,
                Title = deletedBlogpost.Title,
                UrlHandle = deletedBlogpost.UrlHandle,
                ShortDescription = deletedBlogpost.ShortDescription,
                PublishedDate = deletedBlogpost.PublishedDate,
                Content = deletedBlogpost.Content,
                FeaturedImageUrl = deletedBlogpost.FeaturedIamgeUrl,
                IsVisible = deletedBlogpost.IsVisible,
                Author = deletedBlogpost.Author,
                Categories = deletedBlogpost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };

            //Return deleted blog post
            return Ok(response);
        }
    }
}
