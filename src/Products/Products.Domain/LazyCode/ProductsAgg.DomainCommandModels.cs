namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.CommandModels
{
using Queries.Models; 
using Products.Application.DTO.Aggregates.ProductsAgg.Requests; 
public partial class CreateProductsCommand : BaseRequestableCommand<ProductsQueryModel, ProductsDTO>
{
    public bool UpdateIfExists { get; set; }
    public CreateProductsCommand(ILogRequestContext ctx, ProductsDTO data, bool updateIfExists = true, ProductsQueryModel query = null) 
        : base(ctx, query, data) { this.UpdateIfExists = updateIfExists; }
}
public partial class DeleteProductsCommand : BaseDeletionCommand<ProductsQueryModel>
{
    public DeleteProductsCommand(ILogRequestContext ctx, ProductsQueryModel query, bool isLogicalDeletion = true)
        : base(ctx, query, isLogicalDeletion) { }
}
  public partial class DeleteRangeProductsCommand : BaseDeletionCommand<ProductsQueryModel>
{
    public DeleteRangeProductsCommand(ILogRequestContext ctx, ProductsQueryModel query, bool isLogicalDeletion = true)
        : base(ctx, query, isLogicalDeletion) { }
}
public partial class UpdateRangeProductsCommand : BaseRequestableRangeCommand<ProductsQueryModel, ProductsDTO>
{
    public bool CreateIfNotExists { get; set; } = true;
    public UpdateRangeProductsCommand(ILogRequestContext ctx, Dictionary<ProductsQueryModel, ProductsDTO> query)
        : base(ctx, query) { }
    public UpdateRangeProductsCommand(ILogRequestContext ctx, ProductsQueryModel query, ProductsDTO data)
        : base(ctx, new Dictionary<ProductsQueryModel, ProductsDTO> { { query, data } }) { }
}

public partial class UpdateProductsCommand : BaseRequestableCommand<ProductsQueryModel, ProductsDTO>
{
    public bool CreateIfNotExists { get; set; } = true;
    public UpdateProductsCommand(ILogRequestContext ctx, ProductsQueryModel query, ProductsDTO data, bool createIfNotExists = true)
        : base(ctx, query, data) { this.CreateIfNotExists = createIfNotExists; }
}
public partial class ActiveProductsCommand : ProductsSearchableCommand
{
    public ActiveProductsCommand(ILogRequestContext ctx, ProductsQueryModel query)
        : base(ctx, query) { }
}
public partial class DeactiveProductsCommand : ProductsSearchableCommand
{
    public DeactiveProductsCommand(ILogRequestContext ctx, ProductsQueryModel query)
        : base(ctx, query) { }
}
public class ProductsSearchableCommand : BaseSearchableCommand<ProductsQueryModel> {
    public ProductsSearchableCommand(ILogRequestContext ctx, ProductsQueryModel query)
        : base(ctx, query) { }
}

}
