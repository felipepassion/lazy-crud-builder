using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using FluentValidation;

namespace LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>, IValidator<T>
        where T : EntityDTO
    {
        protected HttpClient _http;

        public BaseValidator()
        {

        }
        public BaseValidator(HttpClient db)
        {
            _http = db;
        }

        protected async Task<bool> BeUnique<K>(string externalId, string name, string value, CancellationToken cancellationToken, string query = null)
            where K : EntityDTO, new()
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            var str = $"{typeof(K).Name.Replace("DTO", "")}?{name.ToUpper()}Equal={value}";
            var exist = await _http.GetAsync($"api/{new K().GetType().Name.Replace("DTO", "")}?ExternalIdNotEqual={externalId}&{name.ToUpper()}Equal={value}&{query}");
            return exist.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
    }
}
