using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Events;
using Niu.Nutri.Core.Domain.CrossCutting;
using Niu.Nutri.Core.Domain.Seedwork;
using Niu.Nutri.CrossCutting.Infra.Log.Contexts;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Commands.Handles
{
    public class BaseCommandHandler<T>
        where T : IEntity
    {
        protected readonly IMediator _mediator;
        protected readonly IServiceProvider _serviceProvider;
        readonly protected ValidationResult ValidationResult;

        protected BaseCommandHandler(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            ValidationResult = new ValidationResult();
        }

        protected DomainResponse AddError(string mensagem, string propertyName = null!)
        {
            ValidationResult.Errors.Add(new ValidationFailure(propertyName ?? string.Empty, mensagem));
            return new DomainResponse(this.ValidationResult?.Errors.Select(x => x.ErrorMessage)?.ToArray()!);
        }

        protected DomainResponse AddErrors(Dictionary<string, string> errors)
        {
            foreach (var item in errors)
            {
                ValidationResult.Errors.Add(new ValidationFailure(item.Key ?? string.Empty, item.Value));
            }
            return new DomainResponse(this.ValidationResult?.Errors.Select(x => x.ErrorMessage)?.ToArray()!);
        }

        protected DomainResponse AddErrors(DomainResponse response)
        {
            foreach (var item in response.Errors)
            {
                ValidationResult.Errors.Add(new ValidationFailure(item.Key, item.Value));
            }
            return response;
        }

        protected async Task<DomainResponse> Commit(IUnitOfWork uow, object? data = null!)
        {
            if (ValidationResult.Errors?.Any() == true)
            {
                await _mediator.Publish(new ErrorEvent(this._serviceProvider.GetRequiredService<ILogRequestContext>(), new Exception("Erro ao salvar dados"), $"Commit Error", this.ValidationResult.Errors));
                return new DomainResponse(this.ValidationResult?.Errors.Select(x => x.ErrorMessage)?.ToArray()!);
            }
            else
            {
                return await uow.CommitAsync(data);
            }
        }

        protected void PublishLog<Type>(Type cmd) where Type : BaseCommand => _mediator.Publish(cmd);

        public async virtual Task<DomainResponse> OnCreateAsync(T entity) => await Task.FromResult(DomainResponse.Ok());
        public async virtual Task<DomainResponse> OnAfterCreatedAsync(T entity) => await Task.FromResult(DomainResponse.Ok());
        public async virtual Task<DomainResponse> OnBeforeUpdateAsync(T entity) => await Task.FromResult(DomainResponse.Ok());
        public async virtual Task<DomainResponse> OnUpdateAsync(T entity, T entityAfter) => await Task.FromResult(DomainResponse.Ok());
        public async virtual Task<DomainResponse> OnDeleteAsync(T entity) => await Task.FromResult(DomainResponse.Ok());
    }
}
