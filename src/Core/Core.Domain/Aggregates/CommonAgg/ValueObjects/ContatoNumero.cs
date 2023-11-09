using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class ContactNumero : Entity
    {
        [RequiredT4("'Contact' precisa ser informado")]
        public required string Numero { get; set; }
    }
}
