using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Attribute that indicates an application service should be generated for the decorated aggregate.
    /// </summary>
    /// <remarks>
    /// Apply to a domain aggregate or DTO to instruct T4 templates to create an application
    /// service with common CRUD operations. This attribute currently acts as a marker; add
    /// properties to control generation behavior if needed in the future.
    /// </remarks>
    [H1("Generate App Service")]
    [Category("Code Generation (T4)", "Automatically generate a CRUD AppService for the aggregate.")]
    public class GenerateAppService : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateAppService"/> attribute.
        /// </summary>
        public GenerateAppService()
        {
        }
    }
}
