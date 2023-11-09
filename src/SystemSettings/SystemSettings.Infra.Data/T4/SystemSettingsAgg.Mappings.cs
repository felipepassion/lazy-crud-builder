using LazyCrudBuilder.SystemSettings.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.UsersAgg.Mappings 
{
    public partial class UserProfileAccessMapping : IEntityTypeConfiguration<UserProfileAccess>
    {
        public void Configure(EntityTypeBuilder<UserProfileAccess> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<UserProfileAccess> builder);
    }
    public partial class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<User> builder);
    }
}
namespace LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Mappings 
{
    public partial class SystemPanelSubItemMapping : IEntityTypeConfiguration<SystemPanelSubItem>
    {
        public void Configure(EntityTypeBuilder<SystemPanelSubItem> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanelSubItem> builder);
    }
    public partial class SystemPanelMapping : IEntityTypeConfiguration<SystemPanel>
    {
        public void Configure(EntityTypeBuilder<SystemPanel> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanel> builder);
    }
    public partial class SystemPanelGroupMapping : IEntityTypeConfiguration<SystemPanelGroup>
    {
        public void Configure(EntityTypeBuilder<SystemPanelGroup> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanelGroup> builder);
    }
    public partial class CargaTabelaMapping : IEntityTypeConfiguration<CargaTabela>
    {
        public void Configure(EntityTypeBuilder<CargaTabela> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<CargaTabela> builder);
    }
    public partial class SystemSettingsAggSettingsMapping : IEntityTypeConfiguration<SystemSettingsAggSettings>
    {
        public void Configure(EntityTypeBuilder<SystemSettingsAggSettings> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemSettingsAggSettings> builder);
    }
}
