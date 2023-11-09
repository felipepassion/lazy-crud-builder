using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Notifications;
using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events
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
