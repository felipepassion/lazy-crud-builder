using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public class CheckForgotPasswordCodeParametersValidator : AbstractValidator<CheckForgotPasswordCodeParametersDTO>, IValidator<CheckForgotPasswordCodeParametersDTO>
    {
        public CheckForgotPasswordCodeParametersValidator()
        {
            // RuleFor...
            RuleFor(x => x.Code).NotEmpty();
        }
    }

    public class CheckForgotPasswordCodeParametersDTO
    {
        [Required]
        [DisplayName("Código")]
        public string Code { get; set; }
    }
}
