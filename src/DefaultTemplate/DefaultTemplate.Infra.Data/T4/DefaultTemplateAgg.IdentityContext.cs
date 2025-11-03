
using Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities; 
using Lazy.Crud.DefaultTemplate.Infra.Data.Aggregates.DefaultTemplateAgg.Mappings; 
using Lazy.Crud.DefaultTemplate.Domain.Aggregates.UsersAgg.Entities; 
using Lazy.Crud.DefaultTemplate.Infra.Data.Aggregates.UsersAgg.Mappings; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Lazy.Crud.Core.Infra.Data.Contexts;

namespace Lazy.Crud.DefaultTemplate.Infra.Data.Context
{
	public partial class DefaultTemplateAggContext : BaseContext
	{
		public DbSet<DefaultEntity> DefaultEntity { get; set; }
		public DbSet<DefaultTemplateAggSettings> DefaultTemplateAggSettings { get; set; }
		public DbSet<User> User { get; set; }

		public DefaultTemplateAggContext (MediatR.IMediator mediator, DbContextOptions<DefaultTemplateAggContext> options, IServiceProvider scope)
            : base(mediator, options, scope)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new DefaultEntityMapping());
			builder.ApplyConfiguration(new DefaultTemplateAggSettingsMapping());
			builder.ApplyConfiguration(new UserMapping());
		
			ApplyAdditionalMappings(builder);
			base.OnModelCreating(builder);
		}
		partial void ApplyAdditionalMappings(ModelBuilder modelBuilder);
	}
}
