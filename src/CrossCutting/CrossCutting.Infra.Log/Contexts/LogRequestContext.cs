using System;
using System.Linq;

namespace Niu.Nutri.CrossCutting.Infra.Log.Contexts
{
    public interface ILogRequestContext
    {
        string ServiceName { get; }
        string Topic { get; }
        Guid LogId { get; set; }
        string ShortLogId { get { return LogId.ToString().Split('-').First(); } }
        string OperationName { get; set; }
        void Setup(string topicName, string serviceName);
        void Start();
        int? LoggedUserId { get; set; }
        string? LoggedUserName { get; set; }
    }

    public class LogRequestContext : ILogRequestContext
    {
        public string Topic { get; set; }
        public Guid LogId { get; set; }
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public int? LoggedUserId { get; set; }
        public string? LoggedUserName { get; set; }

        public LogRequestContext()
        {
            RefreshToken();
        }

        public void Setup(string topicName, string serviceName)
        {
            Topic = topicName;
            ServiceName = serviceName;
        }

        public void Start()
        {
            RefreshToken();
        }

        private void RefreshToken()
        {
            LogId = Guid.NewGuid();
        }
    }
}
