using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodServices _paymentMethodServices;

        public PaymentMethodController(IPaymentMethodServices paymentMethodServices)
        {
            _paymentMethodServices = paymentMethodServices;
        }

        [HttpGet("/payment-methods")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var responses = await _paymentMethodServices.GetAll(page, size);
            return (responses != null)? Ok(responses) : StatusCode(500);
        }

        [HttpGet("/payment-methods/{id}")]
        public async Task<IActionResult> GetPaymentMethodById(string id)
        {
            var response = await _paymentMethodServices.GetPaymentMethodById(id);
            return (response != null)? Ok(response) : NotFound();
        }
    }
}
