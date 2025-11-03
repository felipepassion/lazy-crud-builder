namespace Lazy.Crud.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Prevents overwriting the front-end after the first generation when applied to a class.
    /// </summary>
    [Category("Code Generation (T4)", "Prevents replacing front-end files after initial generation.")]
    public class DoNotReplaceAfterGenerated : Attribute
    {

    }
}
