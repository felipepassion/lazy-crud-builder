using LazyCrud.Core.Domain.CrossCutting;
using FluentValidation.Results;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Commands.Responses
{
    public partial class GetHttpResponseDTO : GetHttpResponseDTO<object>
    {
        public GetHttpResponseDTO() : base() { }
        public GetHttpResponseDTO(object response)
            : base(response)
        {
        }
        public GetHttpResponseDTO(HttpStatusCode response, string[] errors)
            : base(response, errors)
        {
        }

        public static GetHttpResponseDTO Ok(object response)
        {
            return new GetHttpResponseDTO(response) { StatusCode = (int)HttpStatusCode.OK };
        }

        public static GetHttpResponseDTO Ok()
        {
            return new GetHttpResponseDTO() { StatusCode = (int)HttpStatusCode.OK };
        }
    }

    public partial class GetHttpResponseDTO<T> : GetDTO<T>
    {
        public GetHttpResponseDTO() : base() { }

        public override bool Success => base.Success && StatusCode >= (int)HttpStatusCode.OK && StatusCode < (int)HttpStatusCode.Ambiguous;

        public int StatusCode { get; set; }

        public GetHttpResponseDTO(T response)
            : base(response)
        {
        }

        public GetHttpResponseDTO(HttpStatusCode response, string[] errors)
        {
            this.StatusCode = (int)response;
            this.Errors = errors;
        }

        public static GetHttpResponseDTO<T> Ok<T>(T response) where T : class
        {
            return new GetHttpResponseDTO<T>(response) { StatusCode = (int)HttpStatusCode.OK };
        }

        public static GetHttpResponseDTO Ok()
        {
            return new GetHttpResponseDTO() { StatusCode = (int)HttpStatusCode.OK };
        }

        public static GetHttpResponseDTO BadRequest(params string[] errors)
        {
            return Error(HttpStatusCode.BadRequest, errors);
        }

        public static GetHttpResponseDTO BadRequest(DomainResponse response)
        {
            return Error(HttpStatusCode.BadRequest, response.Errors.Select(x=>$"{x.Key}: {x.Value}").ToArray());
        }

        public static GetHttpResponseDTO BadRequest(ValidationResult result)
        {
            var errors = result.Errors?.Select(x => x.ErrorMessage)?.ToArray();
            return Error(HttpStatusCode.BadRequest, errors ?? new string[0]);
        }

        public static GetHttpResponseDTO NotFound(params string[] errors)
        {
            return Error(HttpStatusCode.NotFound, errors);
        }

        public static GetHttpResponseDTO Forbidden(params string[] errors)
        {
            return Error(HttpStatusCode.Forbidden, errors);
        }

        public static GetHttpResponseDTO Error(HttpStatusCode statusCode, string[] errors)
        {
            return new GetHttpResponseDTO(statusCode, errors);
        }
        public static GetHttpResponseDTO Error(params string[] errors)
        {
            return new GetHttpResponseDTO(HttpStatusCode.InternalServerError, errors);
        }
        public static GetHttpResponseDTO<T> ErrorTyped<T>(params string[] errors)
        {
            return new GetHttpResponseDTO<T>(HttpStatusCode.InternalServerError, errors);
        }
    }
}
