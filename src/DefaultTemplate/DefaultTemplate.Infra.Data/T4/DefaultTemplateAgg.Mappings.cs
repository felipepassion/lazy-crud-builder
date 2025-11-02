using Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities;
using Niu.Nutri.DefaultTemplate.Domain.Aggregates.UsersAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Niu.Nutri.DefaultTemplate.Infra.Data.Aggregates.DefaultTemplateAgg.Mappings 
{
    public partial class DefaultEntityMapping : IEntityTypeConfiguration<DefaultEntity>
    {
        public void Configure(EntityTypeBuilder<DefaultEntity> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<DefaultEntity> builder);
    }
    public partial class DefaultTemplateAggSettingsMapping : IEntityTypeConfiguration<DefaultTemplateAggSettings>
    {
        public void Configure(EntityTypeBuilder<DefaultTemplateAggSettings> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<DefaultTemplateAggSettings> builder);
    }
}
namespace Niu.Nutri.DefaultTemplate.Infra.Data.Aggregates.UsersAgg.Mappings 
{
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
