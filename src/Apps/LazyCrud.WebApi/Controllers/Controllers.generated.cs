
using Microsoft.AspNetCore.Mvc;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Application.Aggregates.Common;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Commands.Responses;
namespace LazyCrud.Users.Controllers {
	using Application.DTO.Aggregates.UsersAgg.Requests;
	using Domain.Aggregates.UsersAgg.Queries.Models;
	using Application.Aggregates.UsersAgg.AppServices;
	using Domain.Aggregates.UsersAgg.Entities;
	using LazyCrud.Users.Domain.Aggregates.UsersAgg.CommandModels;
	[ApiController]
    [Route("api/[controller]")]
	public partial class UserProfileAccessController : BaseController {
 LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserProfileAccessAppService _writeService;		IUserProfileAccessAppService _userProfileAccessAppService;
		public UserProfileAccessController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			IUserProfileAccessAppService userProfileAccessAppService ,
			LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserProfileAccessAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_userProfileAccessAppService = userProfileAccessAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] UserProfileAccessQueryModel request) {

		    var obj = await _userProfileAccessAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] UserProfileAccessQueryModel request) {

            return GetHttpResponseDTO.Ok(await _userProfileAccessAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] UserProfileAccessQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userProfileAccessAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] UserProfileAccessQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userProfileAccessAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] UserProfileAccessQueryModel request, int? page = null, int? size = null) {

		    var obj = await _userProfileAccessAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<UserProfileAccess>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO request) {

			request.Command = new CreateUserProfileAccessCommand(_logRequestContext, request);
			var result = await _userProfileAccessAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] UserProfileAccessQueryModel request)
        {
            request.Command = new DeleteUserProfileAccessCommand(_logRequestContext, request);
            var result = await _userProfileAccessAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(UserProfileAccessQueryModel request)
        {
            var result = await _userProfileAccessAppService.DeleteRange(request);
            request.Command = new DeleteRangeUserProfileAccessCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class UserCurrentAccessSelectedController : BaseController {
 LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserCurrentAccessSelectedAppService _writeService;		IUserCurrentAccessSelectedAppService _userCurrentAccessSelectedAppService;
		public UserCurrentAccessSelectedController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			IUserCurrentAccessSelectedAppService userCurrentAccessSelectedAppService ,
			LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserCurrentAccessSelectedAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_userCurrentAccessSelectedAppService = userCurrentAccessSelectedAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] UserCurrentAccessSelectedQueryModel request) {

		    var obj = await _userCurrentAccessSelectedAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] UserCurrentAccessSelectedQueryModel request) {

            return GetHttpResponseDTO.Ok(await _userCurrentAccessSelectedAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] UserCurrentAccessSelectedQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userCurrentAccessSelectedAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] UserCurrentAccessSelectedQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userCurrentAccessSelectedAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] UserCurrentAccessSelectedQueryModel request, int? page = null, int? size = null) {

		    var obj = await _userCurrentAccessSelectedAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<UserCurrentAccessSelected>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserCurrentAccessSelectedDTO request) {

			request.Command = new CreateUserCurrentAccessSelectedCommand(_logRequestContext, request);
			var result = await _userCurrentAccessSelectedAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] UserCurrentAccessSelectedQueryModel request)
        {
            request.Command = new DeleteUserCurrentAccessSelectedCommand(_logRequestContext, request);
            var result = await _userCurrentAccessSelectedAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(UserCurrentAccessSelectedQueryModel request)
        {
            var result = await _userCurrentAccessSelectedAppService.DeleteRange(request);
            request.Command = new DeleteRangeUserCurrentAccessSelectedCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class UserProfileListController : BaseController {
 LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserProfileListAppService _writeService;		IUserProfileListAppService _userProfileListAppService;
		public UserProfileListController(
			IServiceProvider scope,
						LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserAppService userService,             ILogRequestContext logRequestContext,
			IUserProfileListAppService userProfileListAppService ,
			LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserProfileListAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_userProfileListAppService = userProfileListAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] UserProfileListQueryModel request) {

		    var obj = await _userProfileListAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] UserProfileListQueryModel request) {

            return GetHttpResponseDTO.Ok(await _userProfileListAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] UserProfileListQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userProfileListAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] UserProfileListQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userProfileListAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] UserProfileListQueryModel request, int? page = null, int? size = null) {

		    var obj = await _userProfileListAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<UserProfileList>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileListDTO request) {

			request.Command = new CreateUserProfileListCommand(_logRequestContext, request);
			var result = await _userProfileListAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] UserProfileListQueryModel request)
        {
            request.Command = new DeleteUserProfileListCommand(_logRequestContext, request);
            var result = await _userProfileListAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(UserProfileListQueryModel request)
        {
            var result = await _userProfileListAppService.DeleteRange(request);
            request.Command = new DeleteRangeUserProfileListCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class UserProfileController : BaseController {
 LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserProfileAppService _writeService;		IUserProfileAppService _userProfileAppService;
		public UserProfileController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			IUserProfileAppService userProfileAppService ,
			LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserProfileAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_userProfileAppService = userProfileAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] UserProfileQueryModel request) {

		    var obj = await _userProfileAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] UserProfileQueryModel request) {

            return GetHttpResponseDTO.Ok(await _userProfileAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] UserProfileQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userProfileAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] UserProfileQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userProfileAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] UserProfileQueryModel request, int? page = null, int? size = null) {

		    var obj = await _userProfileAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<UserProfile>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileDTO request) {

			request.Command = new CreateUserProfileCommand(_logRequestContext, request);
			var result = await _userProfileAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] UserProfileQueryModel request)
        {
            request.Command = new DeleteUserProfileCommand(_logRequestContext, request);
            var result = await _userProfileAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(UserProfileQueryModel request)
        {
            var result = await _userProfileAppService.DeleteRange(request);
            request.Command = new DeleteRangeUserProfileCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class UsersAggSettingsController : BaseController {
 LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUsersAggSettingsAppService _writeService;		IUsersAggSettingsAppService _usersAggSettingsAppService;
		public UsersAggSettingsController(
			IServiceProvider scope,
						LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserAppService userService,             ILogRequestContext logRequestContext,
			IUsersAggSettingsAppService usersAggSettingsAppService ,
			LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUsersAggSettingsAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_usersAggSettingsAppService = usersAggSettingsAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] UsersAggSettingsQueryModel request) {

		    var obj = await _usersAggSettingsAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] UsersAggSettingsQueryModel request) {

            return GetHttpResponseDTO.Ok(await _usersAggSettingsAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] UsersAggSettingsQueryModel request, int page = 0, int size = 5) {

		    var obj = await _usersAggSettingsAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] UsersAggSettingsQueryModel request, int page = 0, int size = 5) {

		    var obj = await _usersAggSettingsAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] UsersAggSettingsQueryModel request, int? page = null, int? size = null) {

		    var obj = await _usersAggSettingsAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<UsersAggSettings>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UsersAggSettingsDTO request) {

			request.Command = new CreateUsersAggSettingsCommand(_logRequestContext, request);
			var result = await _usersAggSettingsAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] UsersAggSettingsQueryModel request)
        {
            request.Command = new DeleteUsersAggSettingsCommand(_logRequestContext, request);
            var result = await _usersAggSettingsAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(UsersAggSettingsQueryModel request)
        {
            var result = await _usersAggSettingsAppService.DeleteRange(request);
            request.Command = new DeleteRangeUsersAggSettingsCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class UserController : BaseController {
 LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserAppService _writeService;		IUserAppService _userAppService;
		public UserController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			IUserAppService userAppService ,
			LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_userAppService = userAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] UserQueryModel request) {

		    var obj = await _userAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] UserQueryModel request) {

            return GetHttpResponseDTO.Ok(await _userAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] UserQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] UserQueryModel request, int page = 0, int size = 5) {

		    var obj = await _userAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] UserQueryModel request, int? page = null, int? size = null) {

		    var obj = await _userAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<User>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserDTO request) {

			request.Command = new CreateUserCommand(_logRequestContext, request);
			var result = await _userAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] UserQueryModel request)
        {
            request.Command = new DeleteUserCommand(_logRequestContext, request);
            var result = await _userAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(UserQueryModel request)
        {
            var result = await _userAppService.DeleteRange(request);
            request.Command = new DeleteRangeUserCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
}
namespace LazyCrud.SystemSettings.Controllers {
	using Application.DTO.Aggregates.SystemSettingsAgg.Requests;
	using Domain.Aggregates.SystemSettingsAgg.Queries.Models;
	using Application.Aggregates.SystemSettingsAgg.AppServices;
	using Domain.Aggregates.SystemSettingsAgg.Entities;
	using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.CommandModels;
	[ApiController]
    [Route("api/[controller]")]
	public partial class SystemPanelSubItemController : BaseController {
 LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemPanelSubItemAppService _writeService;		ISystemPanelSubItemAppService _systemPanelSubItemAppService;
		public SystemPanelSubItemController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ISystemPanelSubItemAppService systemPanelSubItemAppService ,
			LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemPanelSubItemAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_systemPanelSubItemAppService = systemPanelSubItemAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] SystemPanelSubItemQueryModel request) {

		    var obj = await _systemPanelSubItemAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] SystemPanelSubItemQueryModel request) {

            return GetHttpResponseDTO.Ok(await _systemPanelSubItemAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] SystemPanelSubItemQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemPanelSubItemAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] SystemPanelSubItemQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemPanelSubItemAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] SystemPanelSubItemQueryModel request, int? page = null, int? size = null) {

		    var obj = await _systemPanelSubItemAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<SystemPanelSubItem>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO request) {

			request.Command = new CreateSystemPanelSubItemCommand(_logRequestContext, request);
			var result = await _systemPanelSubItemAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] SystemPanelSubItemQueryModel request)
        {
            request.Command = new DeleteSystemPanelSubItemCommand(_logRequestContext, request);
            var result = await _systemPanelSubItemAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(SystemPanelSubItemQueryModel request)
        {
            var result = await _systemPanelSubItemAppService.DeleteRange(request);
            request.Command = new DeleteRangeSystemPanelSubItemCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class SystemPanelController : BaseController {
 LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemPanelAppService _writeService;		ISystemPanelAppService _systemPanelAppService;
		public SystemPanelController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ISystemPanelAppService systemPanelAppService ,
			LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemPanelAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_systemPanelAppService = systemPanelAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] SystemPanelQueryModel request) {

		    var obj = await _systemPanelAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] SystemPanelQueryModel request) {

            return GetHttpResponseDTO.Ok(await _systemPanelAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] SystemPanelQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemPanelAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] SystemPanelQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemPanelAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] SystemPanelQueryModel request, int? page = null, int? size = null) {

		    var obj = await _systemPanelAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<SystemPanel>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelDTO request) {

			request.Command = new CreateSystemPanelCommand(_logRequestContext, request);
			var result = await _systemPanelAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] SystemPanelQueryModel request)
        {
            request.Command = new DeleteSystemPanelCommand(_logRequestContext, request);
            var result = await _systemPanelAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(SystemPanelQueryModel request)
        {
            var result = await _systemPanelAppService.DeleteRange(request);
            request.Command = new DeleteRangeSystemPanelCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class SystemPanelGroupController : BaseController {
 LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemPanelGroupAppService _writeService;		ISystemPanelGroupAppService _systemPanelGroupAppService;
		public SystemPanelGroupController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ISystemPanelGroupAppService systemPanelGroupAppService ,
			LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemPanelGroupAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_systemPanelGroupAppService = systemPanelGroupAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] SystemPanelGroupQueryModel request) {

		    var obj = await _systemPanelGroupAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] SystemPanelGroupQueryModel request) {

            return GetHttpResponseDTO.Ok(await _systemPanelGroupAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] SystemPanelGroupQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemPanelGroupAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] SystemPanelGroupQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemPanelGroupAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] SystemPanelGroupQueryModel request, int? page = null, int? size = null) {

		    var obj = await _systemPanelGroupAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<SystemPanelGroup>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelGroupDTO request) {

			request.Command = new CreateSystemPanelGroupCommand(_logRequestContext, request);
			var result = await _systemPanelGroupAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] SystemPanelGroupQueryModel request)
        {
            request.Command = new DeleteSystemPanelGroupCommand(_logRequestContext, request);
            var result = await _systemPanelGroupAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(SystemPanelGroupQueryModel request)
        {
            var result = await _systemPanelGroupAppService.DeleteRange(request);
            request.Command = new DeleteRangeSystemPanelGroupCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class CargaTabelaController : BaseController {
 LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ICargaTabelaAppService _writeService;		ICargaTabelaAppService _cargaTabelaAppService;
		public CargaTabelaController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ICargaTabelaAppService cargaTabelaAppService ,
			LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ICargaTabelaAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_cargaTabelaAppService = cargaTabelaAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] CargaTabelaQueryModel request) {

		    var obj = await _cargaTabelaAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] CargaTabelaQueryModel request) {

            return GetHttpResponseDTO.Ok(await _cargaTabelaAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] CargaTabelaQueryModel request, int page = 0, int size = 5) {

		    var obj = await _cargaTabelaAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] CargaTabelaQueryModel request, int page = 0, int size = 5) {

		    var obj = await _cargaTabelaAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] CargaTabelaQueryModel request, int? page = null, int? size = null) {

		    var obj = await _cargaTabelaAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<CargaTabela>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.CargaTabelaDTO request) {

			request.Command = new CreateCargaTabelaCommand(_logRequestContext, request);
			var result = await _cargaTabelaAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] CargaTabelaQueryModel request)
        {
            request.Command = new DeleteCargaTabelaCommand(_logRequestContext, request);
            var result = await _cargaTabelaAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(CargaTabelaQueryModel request)
        {
            var result = await _cargaTabelaAppService.DeleteRange(request);
            request.Command = new DeleteRangeCargaTabelaCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class SystemSettingsAggSettingsController : BaseController {
 LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemSettingsAggSettingsAppService _writeService;		ISystemSettingsAggSettingsAppService _systemSettingsAggSettingsAppService;
		public SystemSettingsAggSettingsController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ISystemSettingsAggSettingsAppService systemSettingsAggSettingsAppService ,
			LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices.ISystemSettingsAggSettingsAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_systemSettingsAggSettingsAppService = systemSettingsAggSettingsAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] SystemSettingsAggSettingsQueryModel request) {

		    var obj = await _systemSettingsAggSettingsAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] SystemSettingsAggSettingsQueryModel request) {

            return GetHttpResponseDTO.Ok(await _systemSettingsAggSettingsAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] SystemSettingsAggSettingsQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemSettingsAggSettingsAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] SystemSettingsAggSettingsQueryModel request, int page = 0, int size = 5) {

		    var obj = await _systemSettingsAggSettingsAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] SystemSettingsAggSettingsQueryModel request, int? page = null, int? size = null) {

		    var obj = await _systemSettingsAggSettingsAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<SystemSettingsAggSettings>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemSettingsAggSettingsDTO request) {

			request.Command = new CreateSystemSettingsAggSettingsCommand(_logRequestContext, request);
			var result = await _systemSettingsAggSettingsAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] SystemSettingsAggSettingsQueryModel request)
        {
            request.Command = new DeleteSystemSettingsAggSettingsCommand(_logRequestContext, request);
            var result = await _systemSettingsAggSettingsAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(SystemSettingsAggSettingsQueryModel request)
        {
            var result = await _systemSettingsAggSettingsAppService.DeleteRange(request);
            request.Command = new DeleteRangeSystemSettingsAggSettingsCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
}
namespace LazyCrud.MarketPlace.Controllers {
	using Application.DTO.Aggregates.MarketPlaceAgg.Requests;
	using Domain.Aggregates.MarketPlaceAgg.Queries.Models;
	using Application.Aggregates.MarketPlaceAgg.AppServices;
	using Domain.Aggregates.MarketPlaceAgg.Entities;
	using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.CommandModels;
	[ApiController]
    [Route("api/[controller]")]
	public partial class ProdutoController : BaseController {
 LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.IProdutoAppService _writeService;		IProdutoAppService _produtoAppService;
		public ProdutoController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			IProdutoAppService produtoAppService ,
			LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.IProdutoAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_produtoAppService = produtoAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] ProdutoQueryModel request) {

		    var obj = await _produtoAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] ProdutoQueryModel request) {

            return GetHttpResponseDTO.Ok(await _produtoAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] ProdutoQueryModel request, int page = 0, int size = 5) {

		    var obj = await _produtoAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] ProdutoQueryModel request, int page = 0, int size = 5) {

		    var obj = await _produtoAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] ProdutoQueryModel request, int? page = null, int? size = null) {

		    var obj = await _produtoAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<Produto>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests.ProdutoDTO request) {

			request.Command = new CreateProdutoCommand(_logRequestContext, request);
			var result = await _produtoAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] ProdutoQueryModel request)
        {
            request.Command = new DeleteProdutoCommand(_logRequestContext, request);
            var result = await _produtoAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(ProdutoQueryModel request)
        {
            var result = await _produtoAppService.DeleteRange(request);
            request.Command = new DeleteRangeProdutoCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class MarketPlaceAggSettingsController : BaseController {
 LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.IMarketPlaceAggSettingsAppService _writeService;		IMarketPlaceAggSettingsAppService _marketPlaceAggSettingsAppService;
		public MarketPlaceAggSettingsController(
			IServiceProvider scope,
						LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices.IUserAppService userService,             ILogRequestContext logRequestContext,
			IMarketPlaceAggSettingsAppService marketPlaceAggSettingsAppService ,
			LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.IMarketPlaceAggSettingsAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_marketPlaceAggSettingsAppService = marketPlaceAggSettingsAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] MarketPlaceAggSettingsQueryModel request) {

		    var obj = await _marketPlaceAggSettingsAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] MarketPlaceAggSettingsQueryModel request) {

            return GetHttpResponseDTO.Ok(await _marketPlaceAggSettingsAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] MarketPlaceAggSettingsQueryModel request, int page = 0, int size = 5) {

		    var obj = await _marketPlaceAggSettingsAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] MarketPlaceAggSettingsQueryModel request, int page = 0, int size = 5) {

		    var obj = await _marketPlaceAggSettingsAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] MarketPlaceAggSettingsQueryModel request, int? page = null, int? size = null) {

		    var obj = await _marketPlaceAggSettingsAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<MarketPlaceAggSettings>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests.MarketPlaceAggSettingsDTO request) {

			request.Command = new CreateMarketPlaceAggSettingsCommand(_logRequestContext, request);
			var result = await _marketPlaceAggSettingsAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] MarketPlaceAggSettingsQueryModel request)
        {
            request.Command = new DeleteMarketPlaceAggSettingsCommand(_logRequestContext, request);
            var result = await _marketPlaceAggSettingsAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(MarketPlaceAggSettingsQueryModel request)
        {
            var result = await _marketPlaceAggSettingsAppService.DeleteRange(request);
            request.Command = new DeleteRangeMarketPlaceAggSettingsCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class CarrinhoController : BaseController {
 LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.ICarrinhoAppService _writeService;		ICarrinhoAppService _carrinhoAppService;
		public CarrinhoController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ICarrinhoAppService carrinhoAppService ,
			LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.ICarrinhoAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_carrinhoAppService = carrinhoAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] CarrinhoQueryModel request) {

		    var obj = await _carrinhoAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] CarrinhoQueryModel request) {

            return GetHttpResponseDTO.Ok(await _carrinhoAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] CarrinhoQueryModel request, int page = 0, int size = 5) {

		    var obj = await _carrinhoAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] CarrinhoQueryModel request, int page = 0, int size = 5) {

		    var obj = await _carrinhoAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] CarrinhoQueryModel request, int? page = null, int? size = null) {

		    var obj = await _carrinhoAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<Carrinho>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests.CarrinhoDTO request) {

			request.Command = new CreateCarrinhoCommand(_logRequestContext, request);
			var result = await _carrinhoAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] CarrinhoQueryModel request)
        {
            request.Command = new DeleteCarrinhoCommand(_logRequestContext, request);
            var result = await _carrinhoAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(CarrinhoQueryModel request)
        {
            var result = await _carrinhoAppService.DeleteRange(request);
            request.Command = new DeleteRangeCarrinhoCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
	[ApiController]
    [Route("api/[controller]")]
	public partial class CategoriaprodutoController : BaseController {
 LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.ICategoriaprodutoAppService _writeService;		ICategoriaprodutoAppService _categoriaprodutoAppService;
		public CategoriaprodutoController(
			IServiceProvider scope,
			            ILogRequestContext logRequestContext,
			ICategoriaprodutoAppService categoriaprodutoAppService ,
			LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices.ICategoriaprodutoAppService writeService )
				: base(logRequestContext, scope)
			{ 
				_categoriaprodutoAppService = categoriaprodutoAppService; 
				_writeService = writeService; 
			}	
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] CategoriaprodutoQueryModel request) {

		    var obj = await _categoriaprodutoAppService.Get(request);
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		[HttpGet("count")]
		public async Task<GetHttpResponseDTO<object>> Count([FromQuery] CategoriaprodutoQueryModel request) {

            return GetHttpResponseDTO.Ok(await _categoriaprodutoAppService.CountAsync(request));
        }
		[HttpGet("search")]
		public async Task<GetHttpResponseDTO<object>> Get([FromQuery] CategoriaprodutoQueryModel request, int page = 0, int size = 5) {

		    var obj = await _categoriaprodutoAppService.GetAll(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("summary")]
		public async Task<GetHttpResponseDTO<object>> GetSummary([FromQuery] CategoriaprodutoQueryModel request, int page = 0, int size = 5) {

		    var obj = await _categoriaprodutoAppService.GetAllSummary(request, page, size);
            return GetHttpResponseDTO.Ok(obj);
        }
		[HttpGet("select")]
		public async Task<IActionResult> Select([FromQuery] CategoriaprodutoQueryModel request, int? page = null, int? size = null) {

		    var obj = await _categoriaprodutoAppService.GetAll(request, page, size, request.Selector.GetPropertySelector<Categoriaproduto>());
            return obj == null? NotFound() : Ok(GetHttpResponseDTO.Ok(obj));
        }
		
		[HttpPost]
		public async Task<GetHttpResponseDTO<object>> Post(LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests.CategoriaprodutoDTO request) {

			request.Command = new CreateCategoriaprodutoCommand(_logRequestContext, request);
			var result = await _categoriaprodutoAppService.Create(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
		}
        [HttpDelete("delete")]
		public async Task<GetHttpResponseDTO<object>> Delete([FromQuery] CategoriaprodutoQueryModel request)
        {
            request.Command = new DeleteCategoriaprodutoCommand(_logRequestContext, request);
            var result = await _categoriaprodutoAppService.Delete(request);
            return result.Success ? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
        [HttpDelete("range")]
        public async Task<GetHttpResponseDTO<object>> DeleteRange(CategoriaprodutoQueryModel request)
        {
            var result = await _categoriaprodutoAppService.DeleteRange(request);
            request.Command = new DeleteRangeCategoriaprodutoCommand(_logRequestContext, request);
            return result.Success? GetHttpResponseDTO.Ok(result) : GetHttpResponseDTO.BadRequest(result);
        }
	}
}
