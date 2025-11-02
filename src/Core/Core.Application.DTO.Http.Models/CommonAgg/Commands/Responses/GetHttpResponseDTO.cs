using Niu.Nutri.Core.Domain.CrossCutting;
using System.Net;

namespace Niu.Nutri.Core.Application.DTO.Http.Models.CommonAgg.Commands.Responses
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
            return new GetHttpResponseDTO(response) { StatusCode = HttpStatusCode.OK };
        }

        public static GetHttpResponseDTO Ok(DomainResponse response)
        {
            return new GetHttpResponseDTO(response?.Data!) { StatusCode = HttpStatusCode.OK };
        }
    }

    public partial class GetHttpResponseDTO<T> : GetDTO<T>
    {
        public GetHttpResponseDTO() : base() { }

        public override bool Success => base.Success && StatusCode >= HttpStatusCode.OK && StatusCode < HttpStatusCode.Ambiguous;

        public HttpStatusCode StatusCode { get; set; }

        public GetHttpResponseDTO(T response)
            : base(response)
        {
        }

        public GetHttpResponseDTO(HttpStatusCode response, string[] errors)
        {
            this.StatusCode = response;
            this.Errors = errors;
        }

        public static GetHttpResponseDTO<T> Ok<T>(T response)
        {
            return new GetHttpResponseDTO<T>(response) { StatusCode = HttpStatusCode.OK };
        }

        public static GetHttpResponseDTO Ok()
        {
            return new GetHttpResponseDTO() { StatusCode = HttpStatusCode.OK };
        }

        public static GetHttpResponseDTO BadRequest(params string[] errors)
        {
            return Error(HttpStatusCode.BadRequest, errors);
        }
        public static GetHttpResponseDTO<T> BadRequest<T>(params string[] errors)
        {
            return Error<T>(HttpStatusCode.BadRequest, errors);
        }

        public static GetHttpResponseDTO NotFound(params string[] errors)
        {
            return Error(HttpStatusCode.NotFound, errors);
        }
        public static GetHttpResponseDTO<T> NotFound<T>(params string[] errors)
        {
            return Error<T>(HttpStatusCode.NotFound, errors);
        }

        public static GetHttpResponseDTO Forbidden(params string[] errors)
        {
            return Error(HttpStatusCode.Forbidden, errors);
        }
        public static GetHttpResponseDTO<T> Forbidden<T>(params string[] errors)
        {
            return Error<T>(HttpStatusCode.Forbidden, errors);
        }

        public static GetHttpResponseDTO Error(Exception ex)
        {
            return new GetHttpResponseDTO(HttpStatusCode.InternalServerError, new string[] { ex.Message, ex.InnerException?.Message ?? "" });
        }
        public static GetHttpResponseDTO<T> ErrorTyped<T>(Exception ex)
        {
            return new GetHttpResponseDTO<T>(HttpStatusCode.InternalServerError, new string[] { ex.Message, ex.InnerException?.Message ?? "" });
        }
        public static GetHttpResponseDTO<T> ErrorTyped<T>(HttpStatusCode statusCode, Exception ex)
        {
            return new GetHttpResponseDTO<T>(statusCode, [ex.Message, ex.InnerException?.Message ?? ""]);
        }
        public static GetHttpResponseDTO Error(HttpStatusCode statusCode, string[] errors)
        {
            return new GetHttpResponseDTO(statusCode, errors);
        }
        public static GetHttpResponseDTO<T> Error<T>(HttpStatusCode statusCode, string[] errors)
        {
            return new GetHttpResponseDTO<T>(statusCode, errors);
        }
        public static GetHttpResponseDTO Error(params string[] errors)
        {
            return new GetHttpResponseDTO(HttpStatusCode.InternalServerError, errors);
        }
        public static GetHttpResponseDTO<T> ErrorTyped<T>(params string[] errors)
        {
            return new GetHttpResponseDTO<T>(HttpStatusCode.InternalServerError, errors);
        }

        public static Http.Models.CommonAgg.Commands.Responses.GetHttpResponseDTO BadRequest(DomainResponse response)
        {
            return Error(HttpStatusCode.BadRequest, response.Errors.Select(x => $"{x.Key}: {x.Value}").ToArray());
        }

        //public static Http.Models.CommonAgg.Commands.Responses.GetHttpResponseDTO BadRequest(ValidationResult result)
        //{
        //    var errors = result.Errors?.Select(x => x.ErrorMessage)?.ToArray();
        //    return Error(HttpStatusCode.BadRequest, errors ?? new string[0]);
        //}
    }
}
