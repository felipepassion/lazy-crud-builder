
using Microsoft.AspNetCore.Mvc;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;
using Lazy.Crud.Builder.Application.Aggregates.Common;
using Lazy.Crud.CrossCutting.Infra.Utils.Extensions;
using Lazy.Crud.Builder.Application.Extensions;
using Lazy.Crud.Builder.Application.DTO.Http.Models.CommonAgg.Commands.Responses;
namespace Lazy.Crud.Test.Api.Controllers {
	using Domain.Aggregates.TestAgg.Queries.Models;
	using Application.Aggregates.TestAgg.AppServices;
	using Domain.Aggregates.TestAgg.Entities;
	using Application.DTO.Aggregates.TestAgg.Requests;
	[Microsoft.AspNetCore.Authorization.Authorize]
	[ApiController]
    [Route("api/[controller]")]
	public partial class TestController : BaseMiniController {
		ITestAppService _testAppService;
		public TestController(
			IServiceProvider scope,
            ILogRequestContext logRequestContext,
			ITestAppService testAppService)
				: base(logRequestContext, scope)
			{ 
				_testAppService = testAppService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] TestQueryModel request) {
		    var obj = await _testAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] TestQueryModel request) {
            return GetHttpResponseDTO.Ok(await _testAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] TestQueryModel request, int page = 0, int size = 5) {	    
			var obj = await _testAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<GetHttpResponseDTO<object>> Select([FromQuery] TestQueryModel request, int? page = null, int? size = null) {
		    var obj = await _testAppService.Select(request, request.Selector.GetPropertySelector<Test>());
            return GetHttpResponseDTO.Ok(obj);
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(TestDTO request) {
			var result = await _testAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] TestQueryModel request) {
            var result = await _testAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(TestQueryModel request) {
            var result = await _testAppService.DeleteRange(request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
}
