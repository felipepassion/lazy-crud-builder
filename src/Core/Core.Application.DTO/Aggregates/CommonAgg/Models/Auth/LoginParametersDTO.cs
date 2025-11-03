using FluentValidation;
using Lazy.Crud.Core.Domain.Attributes.Auth;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public class LoginValidator : AbstractValidator<LoginParametersDTO>, IValidator<LoginParametersDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.EmailOrPhone).NotEmpty()
                                        .WithMessage("E-mail ou número deve ser informado.");

            RuleFor(x => x.Password).NotEmpty()
                                    .WithMessage("Senha deve ser informada.");
        }
    }

    public class LoginParametersDTO
    {
        [Required]
        [EmailOrPhone]
        [DisplayName("EmailOrPhone")]
        public string? EmailOrPhone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = true;

        [Required]
        public bool ValidAllrights { get; set; } = false;

    }
}
