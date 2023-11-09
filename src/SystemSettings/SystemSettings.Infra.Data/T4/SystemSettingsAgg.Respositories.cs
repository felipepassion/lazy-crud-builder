using Microsoft.Extensions.Configuration;
using LazyCrudBuilder.Core.Infra.Data.Repositories;
using LazyCrudBuilder.SystemSettings.Infra.Data.Context;

using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Repositories
{
	using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Repositories;
	public partial class SystemPanelSubItemRepository : Repository<SystemPanelSubItem>, ISystemPanelSubItemRepository { public SystemPanelSubItemRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class SystemPanelRepository : Repository<SystemPanel>, ISystemPanelRepository { public SystemPanelRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class SystemPanelGroupRepository : Repository<SystemPanelGroup>, ISystemPanelGroupRepository { public SystemPanelGroupRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class CargaTabelaRepository : Repository<CargaTabela>, ICargaTabelaRepository { public CargaTabelaRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class SystemSettingsAggSettingsRepository : Repository<SystemSettingsAggSettings>, ISystemSettingsAggSettingsRepository { public SystemSettingsAggSettingsRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

}
