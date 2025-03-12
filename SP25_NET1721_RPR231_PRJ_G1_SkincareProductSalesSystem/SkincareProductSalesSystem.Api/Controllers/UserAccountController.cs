using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincarecategoriesalesSystem.Api.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserService _userAccountService;

        public UserAccountController(IUserService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("/useraccounts")]
        public async Task<IActionResult> GetAllUserAccounts([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var response = await _userAccountService.GetAllAsync(page: page, size: size);
            return response != null ? Ok(response) : StatusCode(500);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("/useraccounts/{id}")]
        public async Task<IActionResult> GetUserAccountById([FromRoute] string id)
        {
            var userAccount = await _userAccountService.GetAsync(id);
            return userAccount != null ? Ok(userAccount) : StatusCode(500);
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("/user/profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _userAccountService.GetCustomerProfile();
            return StatusCode(result.Status, result);
        }

        //[HttpPost("/useraccounts")]
        //public async Task<IActionResult> CreateUserAccount([FromBody] CreateUserAccountRequest request)
        //{
        //    var response = await _userAccountService.Create(request);
        //    return response != null ? Ok(response) : StatusCode(500);
        //}

        //[HttpPut("/useraccounts/{id}")]
        //public async Task<IActionResult> UpdateUserAccount([FromRoute] int id, [FromBody] UpdateUserAccountRequest request)
        //{
        //    var response = await _userAccountService.Update(id, request);
        //    return response != null ? Ok(response) : StatusCode(500);
        //}

        //[HttpDelete("/useraccounts/{id}")]
        //public async Task<IActionResult> DeleteUserAccount([FromRoute] int id)
        //{
        //    var response = await _userAccountService.Delete(id);
        //    return response != null ? Ok(response) : StatusCode(500);
        //}
    }
}