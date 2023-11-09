using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.Users.Domain.Aggregates.SystemSettingsAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazyCrud.Users.Infra.Data.Aggregates.UsersAgg.Mappings 
{
    public partial class UserProfileAccessMapping : IEntityTypeConfiguration<UserProfileAccess>
    {
        public void Configure(EntityTypeBuilder<UserProfileAccess> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfileAccess> builder);
    }
    public partial class UserCurrentAccessSelectedMapping : IEntityTypeConfiguration<UserCurrentAccessSelected>
    {
        public void Configure(EntityTypeBuilder<UserCurrentAccessSelected> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserCurrentAccessSelected> builder);
    }
    public partial class UserProfileListMapping : IEntityTypeConfiguration<UserProfileList>
    {
        public void Configure(EntityTypeBuilder<UserProfileList> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfileList> builder);
    }
    public partial class UserProfileMapping : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfile> builder);
    }
    public partial class UsersAggSettingsMapping : IEntityTypeConfiguration<UsersAggSettings>
    {
        public void Configure(EntityTypeBuilder<UsersAggSettings> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UsersAggSettings> builder);
    }
    public partial class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(x => x.Contact).WithOne().HasForeignKey<UserContact>("Id");
            builder.HasOne(x => x.SelectedAccess).WithOne().HasForeignKey<UserCurrentAccessSelected>("Id");
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<User> builder);
    }
    public partial class UserContactMapping : IEntityTypeConfiguration<UserContact>
    {
        public void Configure(EntityTypeBuilder<UserContact> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserContact> builder);
    }
}
namespace LazyCrud.Users.Infra.Data.Aggregates.SystemSettingsAgg.Mappings 
{
    public partial class SystemPanelSubItemMapping : IEntityTypeConfiguration<SystemPanelSubItem>
    {
        public void Configure(EntityTypeBuilder<SystemPanelSubItem> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanelSubItem> builder);
    }
    public partial class SystemPanelMapping : IEntityTypeConfiguration<SystemPanel>
    {
        public void Configure(EntityTypeBuilder<SystemPanel> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanel> builder);
    }
    public partial class SystemPanelGroupMapping : IEntityTypeConfiguration<SystemPanelGroup>
    {
        public void Configure(EntityTypeBuilder<SystemPanelGroup> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanelGroup> builder);
    }
}
