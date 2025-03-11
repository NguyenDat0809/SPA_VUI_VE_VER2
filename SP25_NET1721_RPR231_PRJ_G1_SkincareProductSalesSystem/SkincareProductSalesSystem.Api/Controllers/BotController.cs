using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;

namespace SkincareProductSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        public BotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("/bot/chat")]
        public async Task<IActionResult> GetAll(string prompt)
        {
            var responses = await _chatBotService.ChatWithBot(prompt);
            return StatusCode(responses.Status, responses);
        }

    }
}
