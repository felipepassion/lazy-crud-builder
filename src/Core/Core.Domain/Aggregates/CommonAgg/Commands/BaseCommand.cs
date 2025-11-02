using FluentValidation.Results;
using MediatR;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Notifications;
using Niu.Nutri.Core.Domain.CrossCutting;
using Niu.Nutri.CrossCutting.Infra.Log.Contexts;
using System.Text.Json.Serialization;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Commands
{
    public abstract class BaseCommand : BaseNotification, IRequest<DomainResponse>, IBaseRequest
    {
        protected BaseCommand(ILogRequestContext ctx) 
            : base(ctx)
        {
        }

        [JsonIgnore]
        public ValidationResult? ValidationResult { get; set; }
        public IEntity? Entity { get; set; }

        public virtual bool IsValid()
        {
            return this.ValidationResult?.Errors.Any() != true;
        }
    }
}
