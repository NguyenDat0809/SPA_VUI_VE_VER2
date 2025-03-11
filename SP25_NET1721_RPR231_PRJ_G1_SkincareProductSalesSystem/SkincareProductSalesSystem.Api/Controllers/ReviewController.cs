using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;
using System.Threading.Tasks;

namespace SkincarecategoriesalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/reviews")]
        public async Task<IActionResult> GetAllReviews([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _reviewService.GetAllAsync(page: page, size: size);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/reviews/{id}")]
        public async Task<IActionResult> GetReviewById([FromRoute] string id)
        {
            var review = await _reviewService.GetAsync(id);
            return review != null ? Ok(review) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/reviews/product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(
            [FromRoute] string productId,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var reviews = await _reviewService.GetByProductIdAsync(productId, page, size);
            return reviews != null ? Ok(reviews) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/reviews/customer/{customerId}")]
        public async Task<IActionResult> GetReviewsByCustomerId(
            [FromRoute] string customerId,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var reviews = await _reviewService.GetByCustomerIdAsync(customerId, page, size);
            return reviews != null ? Ok(reviews) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpPost("/reviews")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
        {
            var response = await _reviewService.Create(request);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpPut("/reviews/{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] string id, [FromBody] UpdateReviewRequest request)
        {
            var response = await _reviewService.Update(id, request);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpDelete("/reviews/{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] string id)
        {
            var response = await _reviewService.Delete(id);
            return response != null ? Ok(response) : StatusCode(500);
        }
    }
}
