using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;


namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/brands")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _brandService.GetPaginate(page, size);
            return StatusCode(response.Status, response);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/brands/{id}")]
        public async Task<IActionResult> GetBrandById(string id)
        {
            var response = await _brandService.GetBrandById(id);
            return StatusCode(response.Status, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/brands")]
        public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
        {
            var response = await _brandService.CreateBrand(request);
            return StatusCode(response.Status, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/brands")]
        public async Task<IActionResult> UpdateBrand(UpdateBrandRequest request)
        {
            var response = await _brandService.UpdateBrand(request);
            return StatusCode(response.Status, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/brands/{id}")]
        public async Task<IActionResult> UpdateBrand(string id)
        {
            var response = await _brandService.DeleteBrand(id);
            return StatusCode(response.Status, response);
        }
    }
}
