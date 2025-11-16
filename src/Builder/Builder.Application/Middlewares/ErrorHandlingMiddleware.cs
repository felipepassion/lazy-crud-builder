using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Lazy.Crud.Builder.Application.DTO.Aggregates.CommonAgg.Models;
using Lazy.Crud.Builder.Application.DTO.Http.Models.CommonAgg.Commands.Responses;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Events;
using Lazy.Crud.Builder.Domain.Extensions;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;
using Lazy.Crud.CrossCutting.Infra.Log.Providers;
using System.Net;

namespace Lazy.Crud.Builder.Api.Middlewares
{
    public class ErrorHandlingMiddleware(ILogRequestContext context, ILogProvider logProvider) : IMiddleware
    {
        private readonly ILogProvider _logProvider = logProvider;
        private readonly ILogRequestContext logContext = context;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            
            //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;
            var requestObject = await context.ReadBodyAsString();
            _logProvider.WriteError(new ErrorEvent(requestObject, logContext, exception));

            var result = GetHttpResponseDTO.Error(exception.Message);
            if (exception.InnerException != null)
                result.AddError(exception.InnerException.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

}
