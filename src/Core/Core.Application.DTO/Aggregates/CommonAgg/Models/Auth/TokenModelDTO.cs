using FluentValidation;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public class TokenParametersValidator : AbstractValidator<TokenModelDTO>, IValidator<TokenModelDTO>
    {
        public TokenParametersValidator()
        {
         
        }
    }

    public class TokenModelDTO
    {
        public string UserName { get; set; } = "";
        public string TokenValue { get; set; } = "";
    }
}
