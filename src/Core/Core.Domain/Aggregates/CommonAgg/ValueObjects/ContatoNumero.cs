using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class ContactNumero : Entity
    {
        [RequiredT4("'Contact' precisa ser informado")]
        public required string Numero { get; set; }
    }
}
