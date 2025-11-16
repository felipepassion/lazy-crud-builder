namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks an entity as steppable and provides the number of steps or related quantity.
    /// </summary>
    /// <category>UI/Forms</category>
    public class Steppable : Attribute
    {
        /// <summary>
        /// Gets or sets the quantity or number of steps available.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Steppable"/> attribute with the specified quantity.
        /// </summary>
        /// <param name="quantity">The number of steps or quantity.</param>
        public Steppable(int quantity)
        {
            Quantity = quantity;
        }

        /// <summary>
        /// Initializes a new default instance of the <see cref="Steppable"/> attribute.
        /// </summary>
        public Steppable()
        {

        }
    }
}
