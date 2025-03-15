using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services.ExtendServices;


namespace SkincareProductSalesSystem.Api.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("/cart")]
        public async Task<IActionResult> GetAll()
        {
            var responses = await _cartService.GetUserCartAsync();
            return StatusCode(responses.Status, responses);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("/cart")]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var responses = await _cartService.AddOrUpdateToCartAsync(request);
            return StatusCode(responses.Status, responses);
        }
        [Authorize(Roles = "Customer")]
        [HttpPatch("/cart")]
        public async Task<IActionResult> UpdateToCart(UpdateToCartRequest request)
        {
            var responses = await _cartService.AddOrUpdateToCartAsync(request);
            return StatusCode(responses.Status, responses);
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("/cart/product/{id}")]
        public async Task<IActionResult> RemoveFromCart(string id)
        {
            var responses = await _cartService.RemoveFromCartAsync(id);
            return StatusCode(responses.Status, responses);
        }
    }
}
