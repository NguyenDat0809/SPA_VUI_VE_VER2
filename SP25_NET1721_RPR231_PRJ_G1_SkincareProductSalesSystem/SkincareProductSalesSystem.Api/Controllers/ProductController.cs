using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/products")]
        public async Task<IActionResult> GetAllProduct([FromQuery] GetAllProductQuery query)
        {
            var response = await _productService.GetAllAsync(query);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/products/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] string id)
        {
            var product = await _productService.GetAsync(id);
            return product != null ? Ok(product) : StatusCode(500);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/products")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var response = await _productService.Create(request);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/products/{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] string id, [FromForm] UpdateProductRequest request)
        {
            var response = await _productService.Update(id, request);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/products/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            var response = await _productService.Delete(id);
            return response != null ? Ok(response) : StatusCode(500);
        }


        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:DefaultConnectionString"];
        }

    }
}