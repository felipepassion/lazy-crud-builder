using FluentValidation;
using Niu.Nutri.Core.Application.DTO.Extensions;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models;
using Microsoft.Extensions.Logging;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>, IValidator<T>
        where T : EntityDTO
    {
        protected HttpClient _http = default!;
        ILogger _logger;

        public BaseValidator()
        {
        }

        public BaseValidator(HttpClient? db)
        {
            _http = db;
        }

        public void SetHttp(HttpClient db)
        {
            _http = db;
        }

        protected async Task<bool> BeUnique<K>(string? externalId, string name, object? value, CancellationToken cancellationToken, string? query = null)
            where K : EntityDTO, new()
        {
            if (string.IsNullOrWhiteSpace(value?.ToString())) return true;
            
            if(value is DateTime datetime)
            {
                value = datetime.ToString("yyyy-MM-dd");
            }
            else if (value is DateOnly dateonly)
            {
                value = dateonly.ToString("yyyy-MM-dd");
            }

            var str = $"{typeof(K).Name.Replace("DTO", "")}?{name}Equal={value}";
            var result = await _http.CountAsync<K>($"ExternalIdNotEqual={externalId}&{name.ToUpper()}Equal={value}&{query}");
            
            return result.Success && result.Data == 0;
        }
    }
}
