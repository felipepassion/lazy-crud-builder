using Microsoft.Extensions.Configuration;
using Niu.Nutri.Core.Infra.Data.Repositories;
using Niu.Nutri.DefaultTemplate.Infra.Data.Context;

using Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities;

namespace Niu.Nutri.DefaultTemplate.Infra.Data.Aggregates.DefaultTemplateAgg.Repositories
{
	using Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Repositories;
	public partial class DefaultEntityRepository : Repository<DefaultEntity>, IDefaultEntityRepository { public DefaultEntityRepository(DefaultTemplateAggContext ctx) : base(ctx) { } }

	public partial class DefaultTemplateAggSettingsRepository : Repository<DefaultTemplateAggSettings>, IDefaultTemplateAggSettingsRepository { public DefaultTemplateAggSettingsRepository(DefaultTemplateAggContext ctx) : base(ctx) { } }

}
