using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincarecategoriesalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categorieservice)
        {
            _categoryService = categorieservice;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/categories")]
        public async Task<IActionResult> GetAllCategory([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _categoryService.GetAllAsync(page: page, size: size);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/categories/{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] string id)
        {
            var Category = await _categoryService.GetAsync(id);
            return Category != null ? Ok(Category) : StatusCode(500);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/categories")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequest request)
        {
            var response = await _categoryService.Create(request);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/categories/{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] string id, [FromForm] UpdateCategoryRequest request)
        {
            var response = await _categoryService.Update(id, request);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/categories/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            var response = await _categoryService.Delete(id);
            return response != null ? Ok(response) : StatusCode(500);
        }
    }
}
