using Niu.Nutri.Core.Application.DTO.Attributes;

namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Instructs the generator/mapping to attach the entity to the context on update.
    /// </summary>
    [H1("Attach On Update")]
    [Category("Code Generation (T4)", "Attach entity to the context when updating during generation/mapping.")]
    public class AttatchOnUpdate : Attribute
    {
    }
}
