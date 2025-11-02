using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public class CreatePasswordParametersValidator : AbstractValidator<CreatePasswordParametersDTO>, IValidator<CreatePasswordParametersDTO>
    {
        public CreatePasswordParametersValidator()
        {
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Senha deve ser informada.");

            RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("As senhas devem ser iguais");

            RuleFor(user => user.Password)
                .MinimumLength(8)
                .WithMessage("Senha deve ser maior ou igual a 8 caracteres.")
                .NotEmpty()
                .WithMessage("Senha deve ser informada.")
                .Matches(@"[A-Z]+")
                .WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
                .Matches(@"[@!\\]+")
                .WithMessage("Senha deve conter pelo menos um dos caracteres especiais: (@, !).");
        }
    }

    public class CreatePasswordParametersDTO
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
