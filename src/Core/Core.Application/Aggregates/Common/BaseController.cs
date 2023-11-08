using LazyCrud.Core.Application.Aggregates.Common.Models;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace LazyCrud.Core.Application.Aggregates.Common
{
    public class BaseController : ControllerBase
    {
        protected readonly IServiceProvider _scope;
        protected readonly ILogRequestContext _logRequestContext;
        protected readonly ILoggedUserContext _loggedUserContext;

        public BaseController(ILogRequestContext logRequestContext, IServiceProvider scope)
        {
            _scope = scope;
            _logRequestContext = logRequestContext;
        }
    }
}