using Azure;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentService;

        public PaymentController(IPaymentServices paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("/payments")]
        public async Task<IActionResult> GetAllPaginate([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _paymentService.GetAllPaginate(page, size);
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payments/order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderIdPaginate(string orderId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _paymentService.GetPaymentsByOrderIdPaginate(orderId,page, size);
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payments/{id}")]
        public async Task<IActionResult> GetPaymentById(string id)
        {
            var response = await _paymentService.GetPaymentById(id);
            return (response != null) ? Ok(response) : NotFound();
        }
    }
}
