using Microsoft.AspNetCore.Http;
using Niu.Nutri.Core.Application.Aggregates.Common.Models;
using Niu.Nutri.Core.Application.DTO.Extensions;

namespace Niu.Nutri.Core.Api.Middlewares;

public class LoggedUserContextMiddleware(ILoggedUserContext context) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        context.UserId = int.TryParse(httpContext.User.GetUserId(), out var val) ? val : 0;
        await next(httpContext);
    }
}
