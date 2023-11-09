namespace LazyCrudBuilder.Core.Domain.Attributes.T4
{
    public enum EndpointTypes
    {
        HttpPost,
        HttpGet,
        HttpDelete,
        HttpPut,
        HttpAll,
        HttpListining,
        Count
    }

    public class EndpointsT4 : Attribute
    {
        public EndpointsT4(params EndpointTypes[] attr)
        {

        }

        public EndpointsT4(int order, params EndpointTypes[] attr)
        {
            Order = order;
        }

        public int Order { get; }
    }
}
