using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
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
    [H1("Disable Auto Controllers")]
    [Category("Code Generation (T4)", "Disable automatic controller/endpoint generation for the type.")]
    public class DisableControllers : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableControllers"/> attribute.
        /// </summary>
        public DisableControllers()
        {
        }
    }
}
