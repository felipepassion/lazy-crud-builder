using FluentValidation.Results;
using MediatR;
using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Notifications;
using Lazy.Crud.Core.Domain.CrossCutting;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;
using System.Text.Json.Serialization;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Commands
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
