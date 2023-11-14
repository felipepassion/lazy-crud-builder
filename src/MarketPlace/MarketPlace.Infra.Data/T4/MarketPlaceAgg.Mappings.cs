using LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazyCrud.MarketPlace.Infra.Data.Aggregates.UsersAgg.Mappings 
{
    public partial class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<User> builder);
    }
}
namespace LazyCrud.MarketPlace.Infra.Data.Aggregates.MarketPlaceAgg.Mappings 
{
    public partial class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<Produto> builder);
    }
    public partial class MarketPlaceAggSettingsMapping : IEntityTypeConfiguration<MarketPlaceAggSettings>
    {
        public void Configure(EntityTypeBuilder<MarketPlaceAggSettings> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<MarketPlaceAggSettings> builder);
    }
    public partial class CarrinhoMapping : IEntityTypeConfiguration<Carrinho>
    {
        public void Configure(EntityTypeBuilder<Carrinho> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<Carrinho> builder);
    }
    public partial class CategoriaprodutoMapping : IEntityTypeConfiguration<Categoriaproduto>
    {
        public void Configure(EntityTypeBuilder<Categoriaproduto> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureAdditionalMapping(builder);
        }

		partial void ConfigureAdditionalMapping(EntityTypeBuilder<Categoriaproduto> builder);
    }
}
