using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class PromotionUsageController : ControllerBase
	{
		private IPromotionUsageService _promotionUsageService;

		public PromotionUsageController(IPromotionUsageService promotionUsageService)
		{
			this._promotionUsageService = promotionUsageService;
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreatePromotionUsageRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await _promotionUsageService.Create(request);
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(UpdatePromotionUsageRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await _promotionUsageService.Update(request);
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([Required] string id)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await _promotionUsageService.Delete(id);
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var response = await _promotionUsageService.GetAll();
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			try
			{
				var response = await _promotionUsageService.GetById(id);
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{page}/{size}")]
		public async Task<IActionResult> Get(int page = 1, int size = 10)
		{
			var response = await _promotionUsageService.GetPaginate(page: page, size:size);
			return response != null ? StatusCode(200, response) : StatusCode(500, "No response");
		}
	}
}
