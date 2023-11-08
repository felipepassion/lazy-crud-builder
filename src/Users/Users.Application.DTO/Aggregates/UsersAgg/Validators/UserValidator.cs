namespace LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Validators
{
    using FluentValidation;
    using Requests;
    using LazyCrud.Core.Application.DTO.Validators;
    using LazyCrud.Core.Application.Validators;

    public partial class UserStep1Validator : BaseUsersAggValidator<UserDTO>
    {
        partial void ConfigureAdditionalValidations()
        {
            //RuleFor(user => user.CpfUser).Must(CpfValidator.ValidCPF)
            //                .When(user => !string.IsNullOrWhiteSpace(user.CpfUser))
            //                .WithMessage("O 'CPF' não é válido");

            RuleFor(user => user.Name).Must(NameValidator.ValidName)
                .WithMessage("O 'Name' não é válido");

            RuleFor(user => user.Contact.Email).Must(EmailValidator.ValidEmail)
             .When(user => string.IsNullOrWhiteSpace(user.Contact.Email) == false)
             .WithMessage("E-mail inválido");

            //RuleFor(x => x).Must(x=>x.Accesses?.All(p=>!p.EmpresaId.HasValue && !p.GrupoEmpresaId.HasValue)==true)
            //    .When(x=>x.Accesses != null && x.Accesses.Any())
        }
    }
}
