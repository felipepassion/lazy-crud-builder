using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Notifications;
using LazyCrudBuilder.Core.Domain.CrossCutting;
using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;
using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Commands
{
    public abstract class BaseCommand : BaseNotification, IRequest<DomainResponse>, IBaseRequest
    {
        protected BaseCommand(ILogRequestContext ctx) 
            : base(ctx)
        {
        }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid()
        {
            return !this.ValidationResult.Errors.Any();
        }
    }
}
