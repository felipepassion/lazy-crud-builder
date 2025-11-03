using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Notifications;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Events
{
    public class BaseEvent : BaseNotification
    {
        public object? Data { get; set; }

        public BaseEvent()
        {

        }

        public BaseEvent(ILogRequestContext ctx)
            :base(ctx)
        {
        }

        public BaseEvent(ILogRequestContext context, object data)
            : this(context)
        {
            this.Data = data;
        }
    }
}
