using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.Users.Identity;

namespace LazyCrud.Users.Infra.Data.Aggregates.UsersAgg.Mappings
{
    public partial class UsersAggSettingsMapping : IEntityTypeConfiguration<UsersAggSettings>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<UsersAggSettings> builder)
        {
            builder.HasOne<User>().WithMany();
        }
    }

    public partial class UserMapping : IEntityTypeConfiguration<User>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasOne(x => x.ApplicationUser).WithOne().HasForeignKey<ApplicationUser>(x => x.Id);
            builder.HasMany(x => x.Accesses).WithOne().HasForeignKey(x => x.UserId);
        }
    }

    public partial class UserProfileListMapping : IEntityTypeConfiguration<UserProfileList>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfileList> builder)
        {
            builder.HasMany(x => x.UserProfiles).WithMany(x => x.AccessesList)
                .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(UserProfileList)}{nameof(UserProfile)}",
                    x => x.HasOne<UserProfile>().WithMany(),
                    x => x.HasOne<UserProfileList>().WithMany()
                );
        }
    }

    public partial class UserProfileMapping : IEntityTypeConfiguration<UserProfile>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasMany(x => x.Accesses).WithOne().HasForeignKey(x => x.UserProfileId);
            //builder.HasMany(x=>x.)

            //builder.HasMany(x => x.MenusGroup).WithMany(x => x.UserProfiles)
            //    .UsingEntity<Dictionary<string, object>>(
            //        $"{nameof(UserProfile)}{nameof(SystemPanelGroup)}",
            //        x => x.HasOne<SystemPanelGroup>().WithMany(),
            //        x => x.HasOne<UserProfile>().WithMany());
        }
    }

    public partial class UserProfileAccessMapping : IEntityTypeConfiguration<UserProfileAccess>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfileAccess> builder)
        {
            //builder.HasOne(x => x.SystemPanelGroup).WithMany(x => x.UserProfileAccesses).HasForeignKey(x => x.SystemPanelGroupId);
            //builder.HasOne(x => x.SystemPanel).WithMany(x=>x.AccessesOfMyProfile).HasForeignKey(x => x.SystemPanelId);
            //builder.HasOne(x => x.SystemPanelSubItem).WithMany().HasForeignKey(x => x.SystemPanelSubItemId);
        }
    }
}

namespace LazyCrud.Users.Infra.Data.Aggregates.IdentityAgg.Mappings
{
    public partial class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
        }

        partial void ConfigureAdditionalMapping(EntityTypeBuilder<ApplicationUser> builder);
    }
}