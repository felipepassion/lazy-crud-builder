
using Microsoft.AspNetCore.Mvc;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;
using Lazy.Crud.Builder.Application.Aggregates.Common;
using Lazy.Crud.CrossCutting.Infra.Utils.Extensions;
using Lazy.Crud.Builder.Application.Extensions;
using Lazy.Crud.Builder.Application.DTO.Http.Models.CommonAgg.Commands.Responses;
namespace Lazy.Crud.Tests.Api.Controllers {
	using Domain.Aggregates.TestsAgg.Queries.Models;
	using Application.Aggregates.TestsAgg.AppServices;
	using Domain.Aggregates.TestsAgg.Entities;
	using Application.DTO.Aggregates.TestsAgg.Requests;
	[Microsoft.AspNetCore.Authorization.Authorize]
	[ApiController]
    [Route("api/[controller]")]
	public partial class TestsController : BaseMiniController {
		ITestsAppService _testsAppService;
		public TestsController(
			IServiceProvider scope,
            ILogRequestContext logRequestContext,
			ITestsAppService testsAppService)
				: base(logRequestContext, scope)
			{ 
				_testsAppService = testsAppService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] TestsQueryModel request) {
		    var obj = await _testsAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] TestsQueryModel request) {
            return GetHttpResponseDTO.Ok(await _testsAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] TestsQueryModel request, int page = 0, int size = 5) {	    
			var obj = await _testsAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<GetHttpResponseDTO<object>> Select([FromQuery] TestsQueryModel request, int? page = null, int? size = null) {
		    var obj = await _testsAppService.Select(request, request.Selector.GetPropertySelector<Tests>());
            return GetHttpResponseDTO.Ok(obj);
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(TestsDTO request) {
			var result = await _testsAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] TestsQueryModel request) {
            var result = await _testsAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(TestsQueryModel request) {
            var result = await _testsAppService.DeleteRange(request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
}
