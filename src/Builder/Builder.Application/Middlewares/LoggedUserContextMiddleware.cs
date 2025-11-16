using Microsoft.AspNetCore.Http;
using Lazy.Crud.Builder.Application.Aggregates.Common.Models;
using Lazy.Crud.Builder.Application.DTO.Extensions;

namespace Lazy.Crud.Builder.Api.Middlewares;

public class LoggedUserContextMiddleware(ILoggedUserContext context) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        context.UserId = int.TryParse(httpContext.User.GetUserId(), out var val) ? val : 0;
        await next(httpContext);
    }
}
