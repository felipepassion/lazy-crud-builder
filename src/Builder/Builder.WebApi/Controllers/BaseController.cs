using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lazy.Crud.Builder.Api.Factory;
using Lazy.Crud.Builder.Application.DTO.Aggregates.CommonAgg.Models;
using Lazy.Crud.Builder.Application.DTO.Http.Models.CommonAgg.Commands.Responses;
using Lazy.Crud.CrossCutting.Infra.Utils.Extensions;
using System.Net.Http.Json;

namespace Lazy.Crud.WebApp.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IConfiguration configuration;
        protected HttpClient _http;

        public BaseController(IConfiguration configuration, HttpClient httpClient)
        {
            this.configuration = configuration;
            _http = httpClient;
        }

        public BaseController(IServiceProvider provider)
        {
            this.configuration = provider.GetRequiredService<IConfiguration>();
            _http = HttpFactoryBuilder.Build(this, configuration, provider.GetRequiredService<IHttpContextAccessor>().HttpContext!);
        }

        protected async Task<GetHttpResponseDTO<T>> GetAsync<T>(object request)
            where T : IEntityDTO, new()
        {
            return await GetAsync<T, T>("", request);
        }
         
        protected async Task<GetHttpResponseDTO<T>> GetAsync<T>(string uri, object request)
            where T : IEntityDTO, new()
        {
            return await GetAsync<T, T>(uri, request);
        }
        
        protected async Task<GetHttpResponseDTO<K>> GetAsync<T, K>(string uri, object request)
            where T : IEntityDTO, new()
        {
            var queryStringParam = request.ObjectToQueryString();

            if (!string.IsNullOrWhiteSpace(uri))
                uri = uri.StartsWith("/") ? uri : ("/" + uri);

            var response = await _http.GetAsync($"{new T().GetRoute()}{uri}{queryStringParam}");

            if ((await response.Content.ReadAsStringAsync()).TryDeserializeJSON<GetHttpResponseDTO<K>>(out var val))
            {
                return val;
            }
            else
            {
                return GetHttpResponseDTO.ErrorTyped<K>("Could not parse response: " + await response.Content.ReadAsStringAsync());
            }
        }

        protected async Task<GetHttpResponseDTO<List<T>>> SearchAsync<T>(object? request = null, int? page = 0, int? size = 5)
            where T : EntityDTO, new()
        {
            return await SearchAsync<T>("search", request, page, size);
        }

        protected async Task<GetHttpResponseDTO<List<K>>> SearchAsync<T, K>(object? request = null, int? page = 0, int? size = 5)
    where T : EntityDTO, new()
        {
            return await SearchAsync<T, K>("search", request, page, size);
        }

        protected async Task<GetHttpResponseDTO<List<T>>> SearchAsync<T>(string uri, object? request = null, int? page = 0, int? size = 5)
            where T : EntityDTO, new()
        {
            try
            {
                var queryStringParam = request?.ObjectToQueryString(includeInitial: false);
                uri = uri.StartsWith("/") ? uri : ("/" + uri);

                var response = await _http.GetAsync($"{new T().GetRoute()}{uri}?size={size}&page={page}&{queryStringParam}");

                if ((await response.Content.ReadAsStringAsync()).TryDeserializeJSON<GetHttpResponseDTO<List<T>>>(out var val))
                {
                    return val;
                }
                else
                {
                    var str = await response.Content.ReadAsStringAsync();
                    return GetHttpResponseDTO.ErrorTyped<List<T>>("Could not parse response: " + str);
                }
            }
            catch (Exception ex)
            {
                return GetHttpResponseDTO.ErrorTyped<List<T>>(ex);
            }
        }

        protected async Task<GetHttpResponseDTO<object>> SelectAsync<T>(string uri, object? request = null, int? page = 0, int? size = 5)
           where T : EntityDTO, new()
        {
            var queryStringParam = request?.ObjectToQueryString(includeInitial: false);
            uri = uri.StartsWith("/") ? uri : ("/" + uri);

            var response = await _http.GetAsync($"{new T().GetSelectRoute()}?size={size}&page={page}&{queryStringParam}");
            var test = await response.Content.ReadAsStringAsync();
            if ((await response.Content.ReadAsStringAsync()).TryDeserializeJSON<GetHttpResponseDTO>(out var val))
            {
                return val;
            }
            else
            {
                var str = await response.Content.ReadAsStringAsync();
                return GetHttpResponseDTO.ErrorTyped<object>("Could not parse response: " + str);
            }
        }

        protected async Task<GetHttpResponseDTO<List<K>>> SearchAsync<T, K>(string uri, object? request = null, int? page = 0, int? size = 5)
    where T : EntityDTO, new()
        {
            var queryStringParam = request?.ObjectToQueryString(includeInitial: false);
            uri = uri.StartsWith("/") ? uri : ("/" + uri);

            var response = await _http.GetAsync($"{new T().GetRoute()}{uri}?size={size}&page={page}&{queryStringParam}");

            if ((await response.Content.ReadAsStringAsync()).TryDeserializeJSON<GetHttpResponseDTO<List<K>>>(out var val))
            {
                return val;
            }
            else
            {
                var str = await response.Content.ReadAsStringAsync();
                return GetHttpResponseDTO.ErrorTyped<List<K>>("Could not parse response: " + str);
            }
        }

        protected async Task<GetHttpResponseDTO<T>> PostAsync<T>(object request)
            where T : EntityDTO, new()
        {
            var response = await _http.PostAsJsonAsync(new T().GetRoute(), request);

            var str = await response.Content.ReadAsStringAsync();

            if (str.TryDeserializeJSON<GetHttpResponseDTO<T>>(out var val))
            {
                return val;
            }
            else
            {
                return GetHttpResponseDTO.ErrorTyped<T>("Could not parse response: " + await response.Content.ReadAsStringAsync());
            }
        }

        protected async Task<GetHttpResponseDTO<K>> PostAsync<T, K>(object request, string? route = null)
            where T : EntityDTO, new()
        {
            var response = await _http.PostAsJsonAsync(new T().GetRoute() + "/" + route, request);

            var str = await response.Content.ReadAsStringAsync();

            if (str.TryDeserializeJSON<GetHttpResponseDTO<K>>(out var val))
            {
                return val;
            }
            else
            {
                return GetHttpResponseDTO.ErrorTyped<K>("Could not parse response: " + await response.Content.ReadAsStringAsync());
            }
        }

        protected async Task<GetHttpResponseDTO<T>> DeleteAsync<T>(object request)
            where T : EntityDTO, new()
        {
            var queryStringParam = request.ObjectToQueryString();

            var response = await _http.DeleteAsync($"{new T().GetDeleteRoute()}{queryStringParam}");

            if ((await response.Content.ReadAsStringAsync()).TryDeserializeJSON<GetHttpResponseDTO<T>>(out var val))
            {
                return val;
            }
            else
            {
                return GetHttpResponseDTO.ErrorTyped<T>("Could not parse response: " + await response.Content.ReadAsStringAsync());
            }
        }

        protected GetHttpResponseDTO BadValidationRequest(ModelStateDictionary modelState)
        {
            var errors = modelState?.Values
                .Where(x => x.ValidationState == ModelValidationState.Invalid)
                .Select(x => string.Join(',', x.Errors?.Select(xx => xx.ErrorMessage)!))
                .ToArray();

            return GetHttpResponseDTO.BadRequest(errors ?? new string[] { "Unkown validation error." });
        }

        protected async Task<GetHttpResponseDTO<T>> PostAsync<T>(object request, string route)
            where T : EntityDTO, new()
        {
            var response = await _http.PostAsJsonAsync(new T().GetRoute() + "/" + route, request);

            var str = await response.Content.ReadAsStringAsync();

            if (str.TryDeserializeJSON<GetHttpResponseDTO<T>>(out var val))
            {
                return val;
            }
            else
            {
                return GetHttpResponseDTO.ErrorTyped<T>("Could not parse response: " + await response.Content.ReadAsStringAsync());
            }
        }
    }
}
