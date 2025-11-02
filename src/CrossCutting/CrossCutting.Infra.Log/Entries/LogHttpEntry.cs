using Serilog.Events;
using System.Diagnostics;
using System.Net;

namespace Niu.Nutri.CrossCutting.Infra.Log.Entries
{
    public class LogHttpEntry : LogEntry
    {
        public string Method { get; set; }
        public string IpAddress { get; set; }
        public string Query { get; set; }
        public string MessageType { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }

        public LogHttpEntry(
                string title,
                object? content = null,
                LogEventLevel? lvl = null,
                string? method = null,
                string? ipAddress = null,
                string? action = null,
                string? controller = null,
                string? query = null,
                string? messageType = null,
                string? uri = null,
                string? requestKey = null,
                params KeyValuePair<string, object>[] properties
            ) : base(title, action, content, lvl, null, requestKey, properties)
        {
            Method = method;
            IpAddress = ipAddress;
            Action = action;
            Query = query;
            MessageType = messageType;
            Url = uri;
            Controller = controller;
        }
    }

    public class LogHttpEntryResponse : LogHttpEntry
    {
        public LogHttpEntryResponse(
                HttpStatusCode responseStatusCode,
                Stopwatch timeElapsed,
                string title,
                object? content = null,
                LogEventLevel? lvl = null,
                string? method = null,
                string? ipAddress = null,
                string? action = null,
                string? controller = null,
                string? query = null,
                string? messageType = null,
                string? url = null,
                string? requestKey = null,
                params KeyValuePair<string, object>[] properties
            ) : base(title, content, lvl, method, ipAddress, action, controller, query, messageType, url, requestKey, properties)
        {
            StatusCode = responseStatusCode;
            TimeElapsed = timeElapsed;
            TimeElapsed.Stop();
        }

        public Stopwatch TimeElapsed { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
