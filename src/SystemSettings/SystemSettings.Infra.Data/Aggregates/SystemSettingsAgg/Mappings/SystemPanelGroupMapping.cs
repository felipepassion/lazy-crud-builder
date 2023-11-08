using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazyCrud.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Mappings
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
                );
        }
    }
}
