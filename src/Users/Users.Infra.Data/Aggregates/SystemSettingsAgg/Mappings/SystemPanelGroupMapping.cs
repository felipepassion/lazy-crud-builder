using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrudBuilder.Users.Infra.Data.Aggregates.SystemSettingsAgg.Mappings
{
    public partial class SystemPanelGroupMapping : IEntityTypeConfiguration<SystemPanelGroup>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanelGroup> builder)
        {
            builder.HasMany(x => x.SubItems).WithMany(x => x.GroupOfMenus)
                .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(SystemPanelGroup)}{nameof(SystemPanel)}",
                    x => x.HasOne<SystemPanel>().WithMany(),
                    x => x.HasOne<SystemPanelGroup>().WithMany()
                ).Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }
}
