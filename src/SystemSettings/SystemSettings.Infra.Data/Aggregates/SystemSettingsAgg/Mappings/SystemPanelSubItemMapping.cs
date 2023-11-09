using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazyCrud.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Mappings
{
    public partial class SystemPanelSubItemMapping : IEntityTypeConfiguration<SystemPanelSubItem>
    {
        partial void ConfigureAdditionalMapping(EntityTypeBuilder<SystemPanelSubItem> builder)
        {
            builder.HasOne<SystemPanelSubItem>().WithMany(x => x.SubItems).HasForeignKey(x => x.SystemPanelSubItemId);
            //builder.HasOne<SystemPanel>().WithMany(x=>x.Aggregates).HasForeignKey(x => x.SystemPanelId);
        }
    }
}
