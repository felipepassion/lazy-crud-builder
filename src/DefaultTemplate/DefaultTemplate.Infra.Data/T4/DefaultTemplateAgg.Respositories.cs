using Microsoft.Extensions.Configuration;
using Lazy.Crud.Core.Infra.Data.Repositories;
using Lazy.Crud.DefaultTemplate.Infra.Data.Context;

using Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities;

namespace Lazy.Crud.DefaultTemplate.Infra.Data.Aggregates.DefaultTemplateAgg.Repositories
{
	using Domain.Aggregates.DefaultTemplateAgg.Repositories;
	public partial class DefaultEntityRepository : Repository<DefaultEntity>, IDefaultEntityRepository { public DefaultEntityRepository(DefaultTemplateAggContext ctx) : base(ctx) { } }

	public partial class DefaultTemplateAggSettingsRepository : Repository<DefaultTemplateAggSettings>, IDefaultTemplateAggSettingsRepository { public DefaultTemplateAggSettingsRepository(DefaultTemplateAggContext ctx) : base(ctx) { } }

}
