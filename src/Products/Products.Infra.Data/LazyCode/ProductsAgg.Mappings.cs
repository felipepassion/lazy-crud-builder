using Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities;
namespace Lazy.Crud.Products.Infra.Data.Aggregates.ProductsAgg.Mappings 
{
 public partial class ProductsMapping : IEntityTypeConfiguration<Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities.Products>
{
    public void Configure(EntityTypeBuilder<Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities.Products> builder)
    {
        builder.HasKey(x => x.Id);
        ConfigureAdditionalMapping(builder);
    }
	partial void ConfigureAdditionalMapping(EntityTypeBuilder<Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities.Products> builder);
}
}
