using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services;
using SkincareProductSalesSystem.Services.Models.SkinTestModels;

namespace SkincareProductSalesSystem.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SkinController : ControllerBase
    {
        private readonly ISkinTestService _skinTestService;
        private readonly ISkinTypeService _skinTypeService;

        public SkinController(ISkinTestService skinTestService,
            ISkinTypeService skinTypeService
            )
        {
            _skinTestService = skinTestService;
            _skinTypeService = skinTypeService;
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/skin-tests")]
        public async Task<IActionResult> GetAllSkinTest()
        {
            var responses =  await _skinTestService.GetAllAsync();
            return (responses != null) ? Ok(responses) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/skin-tests/{id}")]
        public IActionResult GetById(string id)
        {
            var response = _skinTestService.GetAsync(id);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/skin-tests/{questionId}/options/{optionId}")]
        public IActionResult GetById(string questionId, string optionId)
        {
            var response = _skinTestService.GetOptionAsync(questionId, optionId);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/skin-tests")]
        public IActionResult CreateSkinTestQuestion(CreateSkinTestRequest request)
        {
            var response = _skinTestService.Create(request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("/skin-tests/result")]
        public IActionResult SubmitSkinTest(SubmitSkinTestRequest request)
        {
            var response = _skinTestService.SubmitSkinTest(request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/skin-tests/{id}/options")]
        public IActionResult CreateSkinTestOption(string id, CreateSkinTestOptionRequest request)
        {
            var response = _skinTestService.CreateOption(id, request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/skin-tests/{id}")]
        public IActionResult UpdateSkinTestQuestion(string id, UpdateSkinTestRequest request)
        {
            var response = _skinTestService.Update(id, request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/skin-tests/{id}/options")]
        public IActionResult UpdateSkinTestOption(string id, UpdateOptionRequest request)
        {
            var response = _skinTestService.UpdateOption(id, request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/skin-tests/{id}")]
        public IActionResult DeleteSkinTestQuestion(string id)
        {
            var response = _skinTestService.Delete(id);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/skin-tests/{questionId}/options/{optionId}")]
        public IActionResult DeleteSkinTestOption(string questionId, string optionId)
        {
            var response = _skinTestService.DeleteOption(questionId, optionId);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/skin-types")]
        public IActionResult GetAllSkinType()
        {
            var responses = _skinTypeService.GetAllAsync();
            return (responses != null) ? Ok(responses.Result) : StatusCode(500);
        }
        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("/skin-types/{id}")]
        public IActionResult GetTypeById(string id)
        {
            var response = _skinTypeService.GetAsync(id);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/skin-types")]
        public IActionResult CreateSkinType(CreateSkinTypeRequest request)
        {
            var response = _skinTypeService.Create(request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/skin-types/{id}")]
        public IActionResult UpdateSkinType(string id, UpdateSkinTypeRequest request)
        {
            var response = _skinTypeService.Update(id, request);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/skin-types/{id}")]
        public IActionResult DeleteSkinType(string id)
        {
            var response = _skinTypeService.Delete(id);
            return (response != null) ? Ok(response.Result) : NotFound();
        }
    }

}
