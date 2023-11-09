using System.ComponentModel;
using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities
{
    public class UserContact : Entity
    {
        [Step(1)]
        [IgnoreValidation]
        public List<ContactNumero>? Contacts { get; set; }

        [DisplayOnList(6)]
        public string ContactNumbers => string.Join("; ", Contacts?.Select(x => x.Numero) ?? new List<string>());

        [Step(1)]
        [RequiredT4]
        [DisplayName("E-Mail"), DisplayOnList(7)]
        public string? Email { get; set; }
    }
}
