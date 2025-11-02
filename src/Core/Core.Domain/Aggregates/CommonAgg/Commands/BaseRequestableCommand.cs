using Niu.Nutri.CrossCutting.Infra.Log.Contexts;
using Newtonsoft.Json;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Commands
{
    public abstract class BaseRequestableCommand<T, R> : BaseCommand
        where T : class
        where R : class
    {
        [JsonIgnore]
        public T Query { get; private set; }
        public R Request { get; private set; }

        public BaseRequestableCommand(ILogRequestContext ctx, T query, R request)
            : base(ctx)
        {
            this.Query = query;
            this.Request = request;
        }
    }

    public abstract class BaseDeletionCommand<R> : BaseSearchableCommand<R>
        where R : class
    {
        public bool IsLogicalDeletion { get; set; }

        protected BaseDeletionCommand(ILogRequestContext ctx, R query, bool isLogicalDeletion)
            : base(ctx, query)
        {
            this.IsLogicalDeletion = IsLogicalDeletion;
        }
    }

    public abstract class BaseRequestableCommand<R> : BaseCommand
        where R : class
    {
        public R Request { get; private set; }

        protected BaseRequestableCommand(ILogRequestContext ctx, R request)
            : base(ctx)
        {
            this.Request = request;
        }
    }

    public abstract class BaseRequestableRangeCommand<T, R> : BaseCommand
            where T : class
            where R : class
    {
        public Dictionary<T, R> Query { get; private set; }

        protected BaseRequestableRangeCommand(ILogRequestContext ctx, Dictionary<T, R> query)
            : base(ctx)
        {
            this.Query = query;
        }
    }
}
