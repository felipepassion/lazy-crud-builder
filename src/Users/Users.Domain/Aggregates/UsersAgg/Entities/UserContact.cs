using System.ComponentModel;
using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities
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
