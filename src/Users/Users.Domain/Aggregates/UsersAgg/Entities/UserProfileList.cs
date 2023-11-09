using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class UserProfileList : SteppableEntity
    {
        public int? UserId { get; set; }

        [ListingPicker]
        public List<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    }
}
