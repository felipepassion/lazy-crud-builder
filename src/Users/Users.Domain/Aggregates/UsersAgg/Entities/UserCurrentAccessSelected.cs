using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class UserCurrentAccessSelected : Entity
    {
        public int? UserProfileId { get; set; }

        [IgnorePropertyT4]
        public UserProfile UserProfile { get; set; }
    }
}
