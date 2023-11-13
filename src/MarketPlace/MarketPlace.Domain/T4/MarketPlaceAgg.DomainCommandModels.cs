using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.CommandModels
{
    using Queries.Models; 
    using LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests; 
    public partial class CreateProdutoCommand : BaseRequestableCommand<ProdutoQueryModel, ProdutoDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateProdutoCommand(ILogRequestContext ctx, ProdutoDTO data, bool updateIfExists = true, ProdutoQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteProdutoCommand : BaseDeletionCommand<ProdutoQueryModel>
    {
        public DeleteProdutoCommand(ILogRequestContext ctx, ProdutoQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeProdutoCommand : BaseDeletionCommand<ProdutoQueryModel>
    {
        public DeleteRangeProdutoCommand(ILogRequestContext ctx, ProdutoQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeProdutoCommand : BaseRequestableRangeCommand<ProdutoQueryModel, ProdutoDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeProdutoCommand(ILogRequestContext ctx, Dictionary<ProdutoQueryModel, ProdutoDTO> query)
            : base(ctx, query) { }
        public UpdateRangeProdutoCommand(ILogRequestContext ctx, ProdutoQueryModel query, ProdutoDTO data)
            : base(ctx, new Dictionary<ProdutoQueryModel, ProdutoDTO> { { query, data } }) { }
    }
    
    public partial class UpdateProdutoCommand : BaseRequestableCommand<ProdutoQueryModel, ProdutoDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateProdutoCommand(ILogRequestContext ctx, ProdutoQueryModel query, ProdutoDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveProdutoCommand : ProdutoSearchableCommand
    {
        public ActiveProdutoCommand(ILogRequestContext ctx, ProdutoQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveProdutoCommand : ProdutoSearchableCommand
    {
        public DeactiveProdutoCommand(ILogRequestContext ctx, ProdutoQueryModel query)
            : base(ctx, query) { }
    }
    public class ProdutoSearchableCommand : BaseSearchableCommand<ProdutoQueryModel> {
        public ProdutoSearchableCommand(ILogRequestContext ctx, ProdutoQueryModel query)
            : base(ctx, query) { }
    }

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

    public partial class CreateCarrinhoCommand : BaseRequestableCommand<CarrinhoQueryModel, CarrinhoDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateCarrinhoCommand(ILogRequestContext ctx, CarrinhoDTO data, bool updateIfExists = true, CarrinhoQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteCarrinhoCommand : BaseDeletionCommand<CarrinhoQueryModel>
    {
        public DeleteCarrinhoCommand(ILogRequestContext ctx, CarrinhoQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeCarrinhoCommand : BaseDeletionCommand<CarrinhoQueryModel>
    {
        public DeleteRangeCarrinhoCommand(ILogRequestContext ctx, CarrinhoQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeCarrinhoCommand : BaseRequestableRangeCommand<CarrinhoQueryModel, CarrinhoDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeCarrinhoCommand(ILogRequestContext ctx, Dictionary<CarrinhoQueryModel, CarrinhoDTO> query)
            : base(ctx, query) { }
        public UpdateRangeCarrinhoCommand(ILogRequestContext ctx, CarrinhoQueryModel query, CarrinhoDTO data)
            : base(ctx, new Dictionary<CarrinhoQueryModel, CarrinhoDTO> { { query, data } }) { }
    }
    
    public partial class UpdateCarrinhoCommand : BaseRequestableCommand<CarrinhoQueryModel, CarrinhoDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateCarrinhoCommand(ILogRequestContext ctx, CarrinhoQueryModel query, CarrinhoDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveCarrinhoCommand : CarrinhoSearchableCommand
    {
        public ActiveCarrinhoCommand(ILogRequestContext ctx, CarrinhoQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveCarrinhoCommand : CarrinhoSearchableCommand
    {
        public DeactiveCarrinhoCommand(ILogRequestContext ctx, CarrinhoQueryModel query)
            : base(ctx, query) { }
    }
    public class CarrinhoSearchableCommand : BaseSearchableCommand<CarrinhoQueryModel> {
        public CarrinhoSearchableCommand(ILogRequestContext ctx, CarrinhoQueryModel query)
            : base(ctx, query) { }
    }

    public partial class CreateCategoriaprodutoCommand : BaseRequestableCommand<CategoriaprodutoQueryModel, CategoriaprodutoDTO>
    {
        public bool UpdateIfExists { get; set; }
        public CreateCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoDTO data, bool updateIfExists = true, CategoriaprodutoQueryModel query = null) 
            : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
    }
    public partial class DeleteCategoriaprodutoCommand : BaseDeletionCommand<CategoriaprodutoQueryModel>
    {
        public DeleteCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
      public partial class DeleteRangeCategoriaprodutoCommand : BaseDeletionCommand<CategoriaprodutoQueryModel>
    {
        public DeleteRangeCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query, bool isLogicalDeletion = true)
            : base(ctx, query, isLogicalDeletion) { }
    }
    public partial class UpdateRangeCategoriaprodutoCommand : BaseRequestableRangeCommand<CategoriaprodutoQueryModel, CategoriaprodutoDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateRangeCategoriaprodutoCommand(ILogRequestContext ctx, Dictionary<CategoriaprodutoQueryModel, CategoriaprodutoDTO> query)
            : base(ctx, query) { }
        public UpdateRangeCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query, CategoriaprodutoDTO data)
            : base(ctx, new Dictionary<CategoriaprodutoQueryModel, CategoriaprodutoDTO> { { query, data } }) { }
    }
    
    public partial class UpdateCategoriaprodutoCommand : BaseRequestableCommand<CategoriaprodutoQueryModel, CategoriaprodutoDTO>
    {
        public bool CreateIfNotExists { get; set; } = true;
        public UpdateCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query, CategoriaprodutoDTO data, bool createIfNotExists = true)
            : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
    }
    public partial class ActiveCategoriaprodutoCommand : CategoriaprodutoSearchableCommand
    {
        public ActiveCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query)
            : base(ctx, query) { }
    }
    public partial class DeactiveCategoriaprodutoCommand : CategoriaprodutoSearchableCommand
    {
        public DeactiveCategoriaprodutoCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query)
            : base(ctx, query) { }
    }
    public class CategoriaprodutoSearchableCommand : BaseSearchableCommand<CategoriaprodutoQueryModel> {
        public CategoriaprodutoSearchableCommand(ILogRequestContext ctx, CategoriaprodutoQueryModel query)
            : base(ctx, query) { }
    }

}
