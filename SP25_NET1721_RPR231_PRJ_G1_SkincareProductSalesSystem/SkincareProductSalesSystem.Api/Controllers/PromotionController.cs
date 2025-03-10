using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/promotions")]
	[ApiController]
	public class PromotionController : ControllerBase
	{
		private IPromotionService _promotionService;

		public PromotionController(IPromotionService promotionService)
		{
			_promotionService = promotionService;
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreatePromotionRequest model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var response = await _promotionService.Create(model);
				return response != null ? 
					StatusCode(response.Status, (response)) 
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
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> Get()
		{
			try
			{
				var response = await _promotionService.GetAll();

				return response != null ?
					StatusCode(response.Status, (response))
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
			try
			{
				var response = await _promotionService.GetPaginate(page : page ,size : size);

				return response != null ?
					StatusCode(200, response)
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
				var response = await _promotionService.GetById(id);

				return response != null ?
					StatusCode(response.Status, (response))
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(DeletePromotionRequest model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var response = await _promotionService.Delete(model);
				return response != null ?
					StatusCode(response.Status, (response))
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
		public async Task<IActionResult> Update(UpdatePromotionRequest model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var response = await _promotionService.Update(model);
				return response != null ?
					StatusCode(response.Status, (response))
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}
	}
}
