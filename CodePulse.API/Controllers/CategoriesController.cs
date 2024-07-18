using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(AppDbContext context, ICategoryRepository categoryRepository)
        {
            this.context = context;
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCateogory(CreateCategoryReqDto reuest)
        {
            //Map DTO to domain 
            var category = new Category
            {
                Name = reuest.Name,
                UrlHandle = reuest.UrlHandle
            };


            var res = await categoryRepository.CreateAsync(category);
            //Map domain model to dto

            var response = new CategoryDto
            {
                Id = res.Id,
                Name = res.Name,
                UrlHandle = res.UrlHandle

            };

            return Ok(response);


        }

        //Get: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCaegories()
        {
            var categories = await categoryRepository.GetCategoriesAsync();

            var response = new List<CategoryDto>();

            //Map domain model to dto
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                    Id = category.Id
                }
                );
            }
            return Ok(response);
        }

        //Get: https://localhost:7112/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var exsistingCategory = await categoryRepository.GetCategoryByIdAsync(id);
            if (exsistingCategory == null)
            {
                return NotFound();
            }

            var resposne = new CategoryDto
            {
                Name = exsistingCategory.Name,
                Id = exsistingCategory.Id,
                UrlHandle = exsistingCategory.UrlHandle
            };

            return Ok(resposne);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategoryById([FromRoute]Guid id, [FromBody]CreateCategoryReqDto category)
        {
            var categoryDomainModel = new Category()
            {
                Id = id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            var res = await categoryRepository.UpdateCategoryByIdAsync(categoryDomainModel);

            if (res == null) { 
                return NotFound();
            }

            var dto = new CategoryDto()
            {
                Id = res.Id,
                Name = res.Name,
                UrlHandle = res.UrlHandle
            };
            return Ok(dto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteCategoryAsync(id);

            if (category == null)
            {
                return NotFound();

            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }
    }
}
