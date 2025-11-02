using Newtonsoft.Json;
using Serilog.Events;
using Niu.Nutri.CrossCutting.Infra.Log.Contexts;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Events
{
    public partial class ErrorEvent : BaseEvent
    {
        public Exception Exception { get; set; }
        public object RequestObject { get; set; }

        public ErrorEvent(object requestObject, ILogRequestContext logRequestContext, Exception ex, LogEventLevel logType = LogEventLevel.Error)
            : base(logRequestContext)
        {
            this.LogType = logType;
            this.Exception = ex;
            this.Title = "Error";
            this.RequestObject = requestObject;
        }

        public ErrorEvent(ILogRequestContext logRequestContext, Exception ex, string title, object? content = null, LogEventLevel logType = LogEventLevel.Error)
            : this(null, logRequestContext, ex, logType)
        {
            this.Title = title;
            if (content != null)
            {
                this.Data = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(content,settings: new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
        }
    }
}
