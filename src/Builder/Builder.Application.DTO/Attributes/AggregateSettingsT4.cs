using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks an aggregate or member with settings intended to be consumed by T4 templates
    /// or code generation pipelines.
    /// </summary>
    /// <remarks>
    /// Use this attribute to signal that a class or property represents an aggregate
    /// that requires special handling during T4 generation or mapping. Currently this
    /// attribute acts as a marker only and does not provide configurable options.
    /// If aggregate-level configuration is needed, extend this attribute with properties
    /// such as generation mode, included members, or serialization behavior.
    /// </remarks>
    /// <category>Code Generation (T4)</category>
    public class AggregateSettingsT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateSettingsT4"/> attribute.
        /// </summary>
        public AggregateSettingsT4()
        {
        }
    }
}
