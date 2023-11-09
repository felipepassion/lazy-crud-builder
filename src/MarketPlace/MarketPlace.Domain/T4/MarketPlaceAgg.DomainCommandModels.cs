using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.CommandModels
{
    using Queries.Models; 
    using LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests; 
    public partial class CreateMarketPlaceAggSettingsCommand : BaseRequestableCommand<MarketPlaceAggSettingsQueryModel, MarketPlaceAggSettingsDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsDTO data, bool updateIfExists = true, MarketPlaceAggSettingsQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteMarketPlaceAggSettingsCommand : BaseDeletionCommand<MarketPlaceAggSettingsQueryModel>
    {
        public DeleteMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeMarketPlaceAggSettingsCommand : BaseDeletionCommand<MarketPlaceAggSettingsQueryModel>
    {
        public DeleteRangeMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeMarketPlaceAggSettingsCommand : BaseRequestableRangeCommand<MarketPlaceAggSettingsQueryModel, MarketPlaceAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeMarketPlaceAggSettingsCommand(ILogRequestContext ctx, Dictionary<MarketPlaceAggSettingsQueryModel, MarketPlaceAggSettingsDTO> query)
            : base(ctx, query) { }
        public UpdateRangeMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query, MarketPlaceAggSettingsDTO data)
            : base(ctx, new Dictionary<MarketPlaceAggSettingsQueryModel, MarketPlaceAggSettingsDTO> { { query, data } }) { }
    }
    
    public partial class UpdateMarketPlaceAggSettingsCommand : BaseRequestableCommand<MarketPlaceAggSettingsQueryModel, MarketPlaceAggSettingsDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query, MarketPlaceAggSettingsDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveMarketPlaceAggSettingsCommand : MarketPlaceAggSettingsSearchableCommand
    {
        public ActiveMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveMarketPlaceAggSettingsCommand : MarketPlaceAggSettingsSearchableCommand
    {
        public DeactiveMarketPlaceAggSettingsCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query)
            : base(ctx, query) { }
    }
    public class MarketPlaceAggSettingsSearchableCommand : BaseSearchableCommand<MarketPlaceAggSettingsQueryModel> {
        public MarketPlaceAggSettingsSearchableCommand(ILogRequestContext ctx, MarketPlaceAggSettingsQueryModel query)
            : base(ctx, query) { }
    }

}
