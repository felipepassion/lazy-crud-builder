using FluentValidation;
using Niu.Nutri.Core.Application.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public class ForgotPasswordParametersValidator : AbstractValidator<ForgotPasswordParametersDTO>, IValidator<ForgotPasswordParametersDTO>
    {
        public ForgotPasswordParametersValidator()
        {
            RuleFor(x => x.EmailOrPhone).NotEmpty()
                                        .WithMessage("E-mail ou número deve ser informado.");
        }
    }

    public class ForgotPasswordParametersDTO
    {
        [Required]
        [DisplayName("E-Mail")]
        public string EmailOrPhone { get; set; } = string.Empty;
    }
}
