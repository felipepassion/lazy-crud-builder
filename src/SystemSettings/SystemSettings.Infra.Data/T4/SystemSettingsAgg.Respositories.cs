using Microsoft.Extensions.Configuration;
using LazyCrud.Core.Infra.Data.Repositories;
using LazyCrud.SystemSettings.Infra.Data.Context;

using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrud.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Repositories
{
	using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Repositories;
	public partial class SystemPanelSubItemRepository : Repository<SystemPanelSubItem>, ISystemPanelSubItemRepository { public SystemPanelSubItemRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class SystemPanelRepository : Repository<SystemPanel>, ISystemPanelRepository { public SystemPanelRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class SystemPanelGroupRepository : Repository<SystemPanelGroup>, ISystemPanelGroupRepository { public SystemPanelGroupRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class CargaTabelaRepository : Repository<CargaTabela>, ICargaTabelaRepository { public CargaTabelaRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

	public partial class SystemSettingsAggSettingsRepository : Repository<SystemSettingsAggSettings>, ISystemSettingsAggSettingsRepository { public SystemSettingsAggSettingsRepository(SystemSettingsAggContext ctx) : base(ctx) { } }

}
