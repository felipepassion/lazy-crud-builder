using Microsoft.EntityFrameworkCore;
using LazyCrudBuilder.Users.Infra.Data.Aggregates.IdentityAgg.Mappings;

namespace LazyCrudBuilder.Users.Infra.Data.Context
{
    public partial class UsersAggContext
    {
        partial void ApplyAdditionalMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserMapping());
        }
    }
}
