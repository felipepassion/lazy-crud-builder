using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Supported endpoint types used to control which HTTP endpoints should be generated
    /// for an aggregate or operation by T4 templates.
    /// </summary>
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

    /// <summary>
    /// Attribute used to indicate which endpoints should be generated for a type.
    /// </summary>
    /// <remarks>
    /// Use this attribute on aggregates or DTOs to restrict or specify the HTTP
    /// endpoints that T4 generators should create. You can optionally provide an
    /// order value to control generation ordering.
    /// </remarks>
    /// <category>Code Generation (T4)</category>
    public class EndpointsT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointsT4"/> attribute with the selected endpoint types.
        /// </summary>
        /// <param name="attr">The endpoint types to enable.</param>
        public EndpointsT4(params EndpointTypes[] attr)
        {
            Types = attr;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointsT4"/> attribute with an explicit order and the selected endpoint types.
        /// </summary>
        /// <param name="order">Order used by generation tools to sequence processing.</param>
        /// <param name="attr">The endpoint types to enable.</param>
        public EndpointsT4(int order, params EndpointTypes[] attr)
        {
            Order = order;
            Types = attr;
        }

        /// <summary>
        /// Gets the ordering value used by generation pipelines.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Gets the endpoint types specified on the attribute.
        /// </summary>
        public EndpointTypes[] Types { get; }
    }
}
