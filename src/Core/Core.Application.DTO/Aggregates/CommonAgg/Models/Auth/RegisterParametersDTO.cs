using FluentValidation;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using Niu.Nutri.Core.Application.DTO.Validators;
using Niu.Nutri.Core.Application.Validators;
using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public partial class RegisterValidator : BaseValidator<RegisterParametersDTO>
    {
        public RegisterValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Nome deve ser informado")
                .Must(NameValidator.ValidName)
                .WithMessage("Name deve ter nome e sobrenome, sem caracteres especiais e números.");

            RuleFor(user => user.Password)
                .MinimumLength(8)
                .WithMessage("Senha deve ser maior ou igual a 8 caracteres.")
                .NotEmpty()
                .WithMessage("Senha deve ser informada.")
                .Matches(@"[A-Z]+")
                .WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
                .Matches(@"[@!\\]+")
                .WithMessage("Senha deve conter pelo menos um dos caracteres especiais: (@, !).");

            RuleFor(user => user.PhoneNumber)
                    .Must(x => x.ExtractNumbers().Length == 11)
                    .WithMessage("Telefone deve conter 11 dígitos.")
                    .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                    .NotEmpty()
                    .WithMessage("Telefone deve ser informado.");

            RuleFor(user => user.PhoneNumber)
                    .Must(PhoneNumberValidator.IsValid)
                    .WithMessage("Telefone inválido")
                    .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber) && x.PhoneNumber.ExtractNumbers().Length == 11);

            RuleFor(user => user.UserName)
                .Must(EmailValidator.ValidEmail)
                .When(user => !string.IsNullOrWhiteSpace(user.UserName))
                .WithMessage("E-Mail inválido")
                .NotEmpty()
                .WithMessage("E-mail deve ser informado.");

            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }

    public class RegisterParametersDTO : EntityDTO
    {
        [Required]
        [EmailAddress]
        [DisplayName("E-Mail")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Telefone")]
        public string PhoneNumber { get; set; }

        public string PhoneNumberWithNoMask => this.PhoneNumber.ExtractNumbers();

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Password { get; set; }

        [Required]
        public bool isChecked { get; set; } = false;

        public override string GetRoute()
        {
            return "api/user";
        }
    }
}
