using Microsoft.EntityFrameworkCore;
using LazyCrud.Users.Infra.Data.Aggregates.IdentityAgg.Mappings;

namespace LazyCrud.Users.Infra.Data.Context
{
    public partial class UsersAggContext
    {
        partial void ApplyAdditionalMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserMapping());
        }
    }
}
