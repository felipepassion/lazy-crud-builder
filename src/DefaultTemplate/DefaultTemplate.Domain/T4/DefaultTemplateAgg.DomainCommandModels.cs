using Niu.Nutri.CrossCutting.Infra.Log.Contexts;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Commands;
namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.CommandModels
{
    using Queries.Models; 
    using Niu.Nutri.DefaultTemplate.Application.DTO.Aggregates.DefaultTemplateAgg.Requests; 
    public partial class CreateDefaultEntityCommand : BaseRequestableCommand<DefaultEntityQueryModel, DefaultEntityDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityDTO data, bool updateIfExists = true, DefaultEntityQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteDefaultEntityCommand : BaseDeletionCommand<DefaultEntityQueryModel>
    {
        public DeleteDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeDefaultEntityCommand : BaseDeletionCommand<DefaultEntityQueryModel>
    {
        public DeleteRangeDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeDefaultEntityCommand : BaseRequestableRangeCommand<DefaultEntityQueryModel, DefaultEntityDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeDefaultEntityCommand(ILogRequestContext ctx, Dictionary<DefaultEntityQueryModel, DefaultEntityDTO> query)
            : base(ctx, query) { }
        public UpdateRangeDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityQueryModel query, DefaultEntityDTO data)
            : base(ctx, new Dictionary<DefaultEntityQueryModel, DefaultEntityDTO> { { query, data } }) { }
    }
    
    public partial class UpdateDefaultEntityCommand : BaseRequestableCommand<DefaultEntityQueryModel, DefaultEntityDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityQueryModel query, DefaultEntityDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveDefaultEntityCommand : DefaultEntitySearchableCommand
    {
        public ActiveDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveDefaultEntityCommand : DefaultEntitySearchableCommand
    {
        public DeactiveDefaultEntityCommand(ILogRequestContext ctx, DefaultEntityQueryModel query)
            : base(ctx, query) { }
    }
    public class DefaultEntitySearchableCommand : BaseSearchableCommand<DefaultEntityQueryModel> {
        public DefaultEntitySearchableCommand(ILogRequestContext ctx, DefaultEntityQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateDefaultTemplateAggSettingsCommand : BaseRequestableCommand<DefaultTemplateAggSettingsQueryModel, DefaultTemplateAggSettingsDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsDTO data, bool updateIfExists = true, DefaultTemplateAggSettingsQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteDefaultTemplateAggSettingsCommand : BaseDeletionCommand<DefaultTemplateAggSettingsQueryModel>
    {
        public DeleteDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeDefaultTemplateAggSettingsCommand : BaseDeletionCommand<DefaultTemplateAggSettingsQueryModel>
    {
        public DeleteRangeDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeDefaultTemplateAggSettingsCommand : BaseRequestableRangeCommand<DefaultTemplateAggSettingsQueryModel, DefaultTemplateAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, Dictionary<DefaultTemplateAggSettingsQueryModel, DefaultTemplateAggSettingsDTO> query)
            : base(ctx, query) { }
        public UpdateRangeDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query, DefaultTemplateAggSettingsDTO data)
            : base(ctx, new Dictionary<DefaultTemplateAggSettingsQueryModel, DefaultTemplateAggSettingsDTO> { { query, data } }) { }
    }
    
    public partial class UpdateDefaultTemplateAggSettingsCommand : BaseRequestableCommand<DefaultTemplateAggSettingsQueryModel, DefaultTemplateAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query, DefaultTemplateAggSettingsDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveDefaultTemplateAggSettingsCommand : DefaultTemplateAggSettingsSearchableCommand
    {
        public ActiveDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveDefaultTemplateAggSettingsCommand : DefaultTemplateAggSettingsSearchableCommand
    {
        public DeactiveDefaultTemplateAggSettingsCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public class DefaultTemplateAggSettingsSearchableCommand : BaseSearchableCommand<DefaultTemplateAggSettingsQueryModel> {
        public DefaultTemplateAggSettingsSearchableCommand(ILogRequestContext ctx, DefaultTemplateAggSettingsQueryModel query)
            : base(ctx, query) { }
    }

}
