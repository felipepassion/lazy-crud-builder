using Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities; 
using Lazy.Crud.Products.Infra.Data.Aggregates.ProductsAgg.Mappings; 
namespace Lazy.Crud.Products.Infra.Data.Context
{
public partial class ProductsAggContext : BaseContext
{
	public DbSet<Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities.Products> Products { get; set; }

	public ProductsAggContext (MediatR.IMediator mediator, DbContextOptions<ProductsAggContext> options, IServiceProvider scope)
        : base(mediator, options, scope)
    { }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new ProductsMapping());
		ApplyAdditionalMappings(builder);
		base.OnModelCreating(builder);
	}
	partial void ApplyAdditionalMappings(ModelBuilder modelBuilder);
}
}
