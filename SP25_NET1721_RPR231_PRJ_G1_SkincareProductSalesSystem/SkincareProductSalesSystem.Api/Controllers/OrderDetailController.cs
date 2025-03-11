using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailServices _orderDetailServices;

        public OrderDetailController(IOrderDetailServices orderDetailServices)
        {
            _orderDetailServices = orderDetailServices;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/order-details/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(string orderId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _orderDetailServices.GetOrderDetailsByOrderId(
                    orderId: orderId,
                    page: page,
                    size: size
                );
            return (responses != null)? Ok(responses) : StatusCode(500);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("/order-details")]
        public async Task<IActionResult> CreateOrderDetail(string orderId, int quanity, Product product)
        {
            var orderDetail = new OrderDetail 
            { 
                OrderDetailId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Discount = 0,
                OrderId = orderId,
                IsReviewed = false,
                ProductId = product.ProductId,
                Quantity = quanity,
                UnitPrice = product.Price,
                UpdatedAt = DateTime.UtcNow,
            };
            var response = await _orderDetailServices.CreateOrderDetail(orderDetail);
            return (response != null)? Ok(response) : StatusCode(300);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpPatch("/order-details")]
        public async Task<IActionResult> UpdateOrderDetail(OrderDetail orderDetail)
        {
            var response = await _orderDetailServices.UpdateOrderDetail(orderDetail);
            return (response != null) ? Ok(response) : StatusCode(300);
        }
    }
}
