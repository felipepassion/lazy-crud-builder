using System.Net;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Commands
{
    public class BaseCommandResponse
    {
        private BaseCommandResponse() { }

        public BaseCommandResponse(object response)
        {
            Data = response;
        }

        public bool Success
        {
            get { return Errors?.Any() != true; }
        }

        public static BaseCommandResponse Error(params string[] errors)
        {
            return new BaseCommandResponse { Errors = errors };
        }

        public void AddError(params string[] newErrors)
        {
            var list = Errors?.ToList() ?? new List<string>();
            list.AddRange(newErrors);
            Errors = list.ToArray();
        }

        public string[] Errors { get; set; }
        public object Data { get; set; }
    }

    public class BaseCommandResponse<T> : BaseCommandResponse
        where T : new()
    {
        public BaseCommandResponse(T response) :
            base(response)
        {
        }
    }
}
