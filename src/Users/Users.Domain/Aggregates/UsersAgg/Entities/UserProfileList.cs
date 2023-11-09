using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class UserProfileList : SteppableEntity
    {
        public int? UserId { get; set; }

        [ListingPicker]
        public List<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    }
}
