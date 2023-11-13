
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities; 
using LazyCrud.MarketPlace.Infra.Data.Aggregates.MarketPlaceAgg.Mappings; 
using LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Entities; 
using LazyCrud.MarketPlace.Infra.Data.Aggregates.UsersAgg.Mappings; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LazyCrud.Core.Infra.Data.Contexts;

namespace LazyCrud.MarketPlace.Infra.Data.Context
{
	public partial class MarketPlaceAggContext : BaseContext
	{
		public DbSet<Produto> Produto { get; set; }
		public DbSet<MarketPlaceAggSettings> MarketPlaceAggSettings { get; set; }
		public DbSet<Carrinho> Carrinho { get; set; }
		public DbSet<Categoriaproduto> Categoriaproduto { get; set; }
		public DbSet<User> User { get; set; }

		public MarketPlaceAggContext (MediatR.IMediator mediator, DbContextOptions<MarketPlaceAggContext> options, IServiceProvider scope)
            : base(mediator, options, scope)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new ProdutoMapping());
			builder.ApplyConfiguration(new MarketPlaceAggSettingsMapping());
			builder.ApplyConfiguration(new CarrinhoMapping());
			builder.ApplyConfiguration(new CategoriaprodutoMapping());
			builder.ApplyConfiguration(new UserMapping());
		
			ApplyAdditionalMappings(builder);
			base.OnModelCreating(builder);
		}
		partial void ApplyAdditionalMappings(ModelBuilder modelBuilder);
	}
}
