using LazyCrud.Core.Domain.Aggregates.CommonAgg.Notifications;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands
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
