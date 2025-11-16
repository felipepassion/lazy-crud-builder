using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Commands;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;
using MediatR;

namespace Lazy.Crud.Builder.Application.Aggregates.Common
{
    public interface IBaseAppService : IDisposable 
    {
    }

    public class BaseAppService(ILogRequestContext logRequestContext, IMediator mediator)
    {
        protected readonly IMediator _mediator = mediator;
        protected readonly ILogRequestContext _logRequestContext = logRequestContext;

        protected void SendCommand<T>(T command)
            where T : BaseCommand
        {
            _mediator.Send(command);
        }
    }
}
