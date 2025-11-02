
using Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities; 
using Niu.Nutri.DefaultTemplate.Infra.Data.Aggregates.DefaultTemplateAgg.Mappings; 
using Niu.Nutri.DefaultTemplate.Domain.Aggregates.UsersAgg.Entities; 
using Niu.Nutri.DefaultTemplate.Infra.Data.Aggregates.UsersAgg.Mappings; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Niu.Nutri.Core.Infra.Data.Contexts;

namespace Niu.Nutri.DefaultTemplate.Infra.Data.Context
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
