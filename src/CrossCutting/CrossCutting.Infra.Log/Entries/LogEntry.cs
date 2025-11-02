using Newtonsoft.Json;
using Serilog.Events;

namespace Niu.Nutri.CrossCutting.Infra.Log.Entries
{
    public class LogEntry
    {
        public LogEntry()
        {

        }
        public LogEntry(
                string title,
                string? action,
                object? content = null,
                LogEventLevel? lvl = null,
                Exception? exception = null,
                string? requestKey = null,
                params KeyValuePair<string, object>[] properties)
        {
            Title = title;
            Action = action;
            Content = content;
            Level = lvl ?? LogEventLevel.Information;
            Properties = new Dictionary<string, object>(properties?.Length ?? 0);
            RequestKey = requestKey;
            Exception = exception;

            if (properties != null)
                foreach (var item in properties)
                {
                    Properties.Add(item.Key, JsonConvert.SerializeObject(item.Value));
                }
        }

        public string LogId { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public object? Content { get; set; }
        public string RequestKey { get; set; }
        public LogEventLevel Level { get; set; }
        public Exception Exception { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}

