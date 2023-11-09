using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Commands;
namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.CommandModels
{
    using Queries.Models; 
    using LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests; 
    public partial class CreateSystemPanelSubItemCommand : BaseRequestableCommand<SystemPanelSubItemQueryModel, SystemPanelSubItemDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemDTO data, bool updateIfExists = true, SystemPanelSubItemQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteSystemPanelSubItemCommand : BaseDeletionCommand<SystemPanelSubItemQueryModel>
    {
        public DeleteSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeSystemPanelSubItemCommand : BaseDeletionCommand<SystemPanelSubItemQueryModel>
    {
        public DeleteRangeSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeSystemPanelSubItemCommand : BaseRequestableRangeCommand<SystemPanelSubItemQueryModel, SystemPanelSubItemDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeSystemPanelSubItemCommand(ILogRequestContext ctx, Dictionary<SystemPanelSubItemQueryModel, SystemPanelSubItemDTO> query)
            : base(ctx, query) { }
        public UpdateRangeSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query, SystemPanelSubItemDTO data)
            : base(ctx, new Dictionary<SystemPanelSubItemQueryModel, SystemPanelSubItemDTO> { { query, data } }) { }
    }
    
    public partial class UpdateSystemPanelSubItemCommand : BaseRequestableCommand<SystemPanelSubItemQueryModel, SystemPanelSubItemDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query, SystemPanelSubItemDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveSystemPanelSubItemCommand : SystemPanelSubItemSearchableCommand
    {
        public ActiveSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveSystemPanelSubItemCommand : SystemPanelSubItemSearchableCommand
    {
        public DeactiveSystemPanelSubItemCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query)
            : base(ctx, query) { }
    }
    public class SystemPanelSubItemSearchableCommand : BaseSearchableCommand<SystemPanelSubItemQueryModel> {
        public SystemPanelSubItemSearchableCommand(ILogRequestContext ctx, SystemPanelSubItemQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateSystemPanelCommand : BaseRequestableCommand<SystemPanelQueryModel, SystemPanelDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateSystemPanelCommand(ILogRequestContext ctx, SystemPanelDTO data, bool updateIfExists = true, SystemPanelQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteSystemPanelCommand : BaseDeletionCommand<SystemPanelQueryModel>
    {
        public DeleteSystemPanelCommand(ILogRequestContext ctx, SystemPanelQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeSystemPanelCommand : BaseDeletionCommand<SystemPanelQueryModel>
    {
        public DeleteRangeSystemPanelCommand(ILogRequestContext ctx, SystemPanelQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeSystemPanelCommand : BaseRequestableRangeCommand<SystemPanelQueryModel, SystemPanelDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeSystemPanelCommand(ILogRequestContext ctx, Dictionary<SystemPanelQueryModel, SystemPanelDTO> query)
            : base(ctx, query) { }
        public UpdateRangeSystemPanelCommand(ILogRequestContext ctx, SystemPanelQueryModel query, SystemPanelDTO data)
            : base(ctx, new Dictionary<SystemPanelQueryModel, SystemPanelDTO> { { query, data } }) { }
    }
    
    public partial class UpdateSystemPanelCommand : BaseRequestableCommand<SystemPanelQueryModel, SystemPanelDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateSystemPanelCommand(ILogRequestContext ctx, SystemPanelQueryModel query, SystemPanelDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveSystemPanelCommand : SystemPanelSearchableCommand
    {
        public ActiveSystemPanelCommand(ILogRequestContext ctx, SystemPanelQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveSystemPanelCommand : SystemPanelSearchableCommand
    {
        public DeactiveSystemPanelCommand(ILogRequestContext ctx, SystemPanelQueryModel query)
            : base(ctx, query) { }
    }
    public class SystemPanelSearchableCommand : BaseSearchableCommand<SystemPanelQueryModel> {
        public SystemPanelSearchableCommand(ILogRequestContext ctx, SystemPanelQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateSystemPanelGroupCommand : BaseRequestableCommand<SystemPanelGroupQueryModel, SystemPanelGroupDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupDTO data, bool updateIfExists = true, SystemPanelGroupQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteSystemPanelGroupCommand : BaseDeletionCommand<SystemPanelGroupQueryModel>
    {
        public DeleteSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeSystemPanelGroupCommand : BaseDeletionCommand<SystemPanelGroupQueryModel>
    {
        public DeleteRangeSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeSystemPanelGroupCommand : BaseRequestableRangeCommand<SystemPanelGroupQueryModel, SystemPanelGroupDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeSystemPanelGroupCommand(ILogRequestContext ctx, Dictionary<SystemPanelGroupQueryModel, SystemPanelGroupDTO> query)
            : base(ctx, query) { }
        public UpdateRangeSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query, SystemPanelGroupDTO data)
            : base(ctx, new Dictionary<SystemPanelGroupQueryModel, SystemPanelGroupDTO> { { query, data } }) { }
    }
    
    public partial class UpdateSystemPanelGroupCommand : BaseRequestableCommand<SystemPanelGroupQueryModel, SystemPanelGroupDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query, SystemPanelGroupDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveSystemPanelGroupCommand : SystemPanelGroupSearchableCommand
    {
        public ActiveSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveSystemPanelGroupCommand : SystemPanelGroupSearchableCommand
    {
        public DeactiveSystemPanelGroupCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query)
            : base(ctx, query) { }
    }
    public class SystemPanelGroupSearchableCommand : BaseSearchableCommand<SystemPanelGroupQueryModel> {
        public SystemPanelGroupSearchableCommand(ILogRequestContext ctx, SystemPanelGroupQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateCargaTabelaCommand : BaseRequestableCommand<CargaTabelaQueryModel, CargaTabelaDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaDTO data, bool updateIfExists = true, CargaTabelaQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteCargaTabelaCommand : BaseDeletionCommand<CargaTabelaQueryModel>
    {
        public DeleteCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeCargaTabelaCommand : BaseDeletionCommand<CargaTabelaQueryModel>
    {
        public DeleteRangeCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeCargaTabelaCommand : BaseRequestableRangeCommand<CargaTabelaQueryModel, CargaTabelaDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeCargaTabelaCommand(ILogRequestContext ctx, Dictionary<CargaTabelaQueryModel, CargaTabelaDTO> query)
            : base(ctx, query) { }
        public UpdateRangeCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaQueryModel query, CargaTabelaDTO data)
            : base(ctx, new Dictionary<CargaTabelaQueryModel, CargaTabelaDTO> { { query, data } }) { }
    }
    
    public partial class UpdateCargaTabelaCommand : BaseRequestableCommand<CargaTabelaQueryModel, CargaTabelaDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaQueryModel query, CargaTabelaDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveCargaTabelaCommand : CargaTabelaSearchableCommand
    {
        public ActiveCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveCargaTabelaCommand : CargaTabelaSearchableCommand
    {
        public DeactiveCargaTabelaCommand(ILogRequestContext ctx, CargaTabelaQueryModel query)
            : base(ctx, query) { }
    }
    public class CargaTabelaSearchableCommand : BaseSearchableCommand<CargaTabelaQueryModel> {
        public CargaTabelaSearchableCommand(ILogRequestContext ctx, CargaTabelaQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateSystemSettingsAggSettingsCommand : BaseRequestableCommand<SystemSettingsAggSettingsQueryModel, SystemSettingsAggSettingsDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsDTO data, bool updateIfExists = true, SystemSettingsAggSettingsQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteSystemSettingsAggSettingsCommand : BaseDeletionCommand<SystemSettingsAggSettingsQueryModel>
    {
        public DeleteSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeSystemSettingsAggSettingsCommand : BaseDeletionCommand<SystemSettingsAggSettingsQueryModel>
    {
        public DeleteRangeSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeSystemSettingsAggSettingsCommand : BaseRequestableRangeCommand<SystemSettingsAggSettingsQueryModel, SystemSettingsAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeSystemSettingsAggSettingsCommand(ILogRequestContext ctx, Dictionary<SystemSettingsAggSettingsQueryModel, SystemSettingsAggSettingsDTO> query)
            : base(ctx, query) { }
        public UpdateRangeSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query, SystemSettingsAggSettingsDTO data)
            : base(ctx, new Dictionary<SystemSettingsAggSettingsQueryModel, SystemSettingsAggSettingsDTO> { { query, data } }) { }
    }
    
    public partial class UpdateSystemSettingsAggSettingsCommand : BaseRequestableCommand<SystemSettingsAggSettingsQueryModel, SystemSettingsAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query, SystemSettingsAggSettingsDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveSystemSettingsAggSettingsCommand : SystemSettingsAggSettingsSearchableCommand
    {
        public ActiveSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveSystemSettingsAggSettingsCommand : SystemSettingsAggSettingsSearchableCommand
    {
        public DeactiveSystemSettingsAggSettingsCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public class SystemSettingsAggSettingsSearchableCommand : BaseSearchableCommand<SystemSettingsAggSettingsQueryModel> {
        public SystemSettingsAggSettingsSearchableCommand(ILogRequestContext ctx, SystemSettingsAggSettingsQueryModel query)
            : base(ctx, query) { }
    }

}
