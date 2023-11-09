using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class UserCurrentAccessSelected : Entity
    {
        public int? UserProfileId { get; set; }

        [IgnorePropertyT4]
        public UserProfile UserProfile { get; set; }
    }
}
