﻿using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using MediatR;

namespace LazyCrud.Core.Application.Aggregates.Common
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
