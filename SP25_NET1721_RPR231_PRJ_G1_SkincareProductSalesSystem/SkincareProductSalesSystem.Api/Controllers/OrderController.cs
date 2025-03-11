using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderServices;

        public OrderController(IOrderService orderServices)
        {
            _orderServices = orderServices;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/orders")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _orderServices.GetPagination(page, size);
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var response = await _orderServices.GetOrderById(id);
            return (response != null) ? Ok(response) : NotFound();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("/orders/order")]
        public async Task<IActionResult> CreateOrder(string id)
        {
            var response = await _orderServices.CreateOrder();
            return (response != null) ? Ok(response) : StatusCode(300);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpPut("/orders")]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            var response = await _orderServices.UpdateOrder(order);
            return (response != null) ? Ok(response) : StatusCode(300);
        }
    }
}
