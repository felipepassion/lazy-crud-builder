        
namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.CommandHandlers {
    using Entities;
    using LazyCrud.Core.Domain.CrossCutting;
    using System.Threading.Tasks;

    public partial class SystemPanelGroupCommandHandler : BaseSystemSettingsAggCommandHandler<SystemPanelGroup>
	{
        public override Task<DomainResponse> OnBeforeUpdateAsync(SystemPanelGroup entity)
        {
            //entity.SubItems.ForEach(x => x.GroupOfMenus = null);
            //entity.AccessesOfMyProfile = null;
            return base.OnBeforeUpdateAsync(entity);
        }
    }
}
