namespace Lazy.Crud.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Prevents overwriting the base registration page on the front-end after the initial generation.
    /// </summary>
    [Category("Code Generation (T4)", "Prevents replacing the base registration page after generation.")]
    public class DoNotReplaceBaseCadastroPageAfterGenerated : Attribute
    {

    }
}
