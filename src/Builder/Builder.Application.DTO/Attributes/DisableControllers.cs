using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Attribute used to indicate that automatically generated controllers should be disabled
    /// for the decorated aggregate or type when T4 templates or generation pipelines run.
    /// </summary>
    /// <remarks>
    /// Apply this attribute to an aggregate root or DTO to prevent generation of controllers
    /// or endpoints for the marked type. This is a marker attribute and does not expose
    /// additional configuration properties.
    /// </remarks>
    /// <category>Code Generation (T4)</category>
    public class DisableControllers : Attribute
    {
        public DisableControllers()
        {
        }
    }
}
