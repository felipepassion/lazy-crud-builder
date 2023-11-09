using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Users.Identity
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public ApplicationUser()
        {
            this.ExternalId = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.UtcNow;
        }

        public string Name { get; set; }

        [IgnorePropertyT4OnRequest]
        [DisplayName("Criado em")]
        public DateTime CreatedAt { get; set; }

        [IgnorePropertyT4OnRequest]
        [DisplayName("Atualizado em")]
        public DateTime? UpdatedAt { get; set; }

        [IgnorePropertyT4OnRequest]
        [DisplayName("Deletado em")]
        public DateTime? DeletedAt { get; set; }
        
        public string ExternalId { get; set; }

        [IgnorePropertyT4OnRequest]
        public bool IsDeleted { get; set; }

        //[Column(TypeName = "jsonb"), NotMapped]
        //public List<EventsHistory>? EventsHistory { get; set; }
        public int? RecoverPasswordCode { get; set; }
        public DateTime? RecoverPasswordCodeDate { get; set; }

        public string GetTitle()
        {
            return string.Empty;
        }

        public string GetTitlePropName()
        {
            return string.Empty;
        }
    }
}
