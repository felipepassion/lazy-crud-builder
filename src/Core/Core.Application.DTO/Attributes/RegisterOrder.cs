namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Defines the registration/display order for menus or navigation flows.
    /// </summary>
    [Category("UI/Navigation", "Defines the registration/display order in menus or flows.")]
    public class RegisterOrder : Attribute
    {
        /// <summary>
        /// Zero-based position used to sort items.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Creates a new <see cref="RegisterOrder"/> with the given position.
        /// </summary>
        /// <param name="position">Sorting position.</param>
        public RegisterOrder(int position)
        {
            Position = position;
        }
    }
}
