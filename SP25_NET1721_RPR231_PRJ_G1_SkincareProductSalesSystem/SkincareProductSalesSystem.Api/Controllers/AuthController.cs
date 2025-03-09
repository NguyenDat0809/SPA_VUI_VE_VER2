using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;
using SkincareProductSalesSystem.Services.Base;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthController : ControllerBase
	{
		private IAuthService _authService;

		public AuthController(IAuthService userService)
		{
			_authService = userService;
		}

		[HttpPost("/register")]
		public async Task<IActionResult> Register(Services.RegisterRequest model)
		{
			if (!ModelState.IsValid) 
				return BadRequest(ModelState);
			try
			{
				var response = await _authService.Register(model);
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

		[HttpPost("/login")]
		public async Task<IActionResult> Login(LoginRequestModel model)
		{
			if (!ModelState.IsValid) 
				return BadRequest(ModelState);
			try
			{
				var response = await _authService.LoginByUsername(model);
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

		[HttpPost("/forgot-password")]
		public async Task<IActionResult> ForgotPassword([Required] string username)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await _authService.ForgotPassword(username);
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

		[HttpPost("/reset-password")]
		public async Task<IActionResult> ResetPassword(Services.ResetPasswordRequest model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await _authService.ResetPassword(model);
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
	}
}
