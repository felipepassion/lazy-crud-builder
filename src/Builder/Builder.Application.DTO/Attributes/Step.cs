using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property or entity step with a specific position used by multi-step forms or processes.
    /// </summary>
    /// <category>UI/Forms</category>
    public class Step : Attribute
    {
        /// <summary>
        /// Gets or sets the position of the step.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Step"/> attribute with the given position.
        /// </summary>
        /// <param name="position">The position of the step.</param>
        public Step(int position)
        {
            Position = position;
        }
    }
}
