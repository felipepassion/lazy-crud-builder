
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.UsersAgg.Entities; 
using LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.UsersAgg.Mappings; 
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities; 
using LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Mappings; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LazyCrudBuilder.Core.Infra.Data.Contexts;

namespace LazyCrudBuilder.SystemSettings.Infra.Data.Context
{
	public partial class SystemSettingsAggContext : BaseContext
	{
		public DbSet<UserProfileAccess> UserProfileAccess { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<SystemPanelSubItem> SystemPanelSubItem { get; set; }
		public DbSet<SystemPanel> SystemPanel { get; set; }
		public DbSet<SystemPanelGroup> SystemPanelGroup { get; set; }
		public DbSet<CargaTabela> CargaTabela { get; set; }
		public DbSet<SystemSettingsAggSettings> SystemSettingsAggSettings { get; set; }

		public SystemSettingsAggContext (MediatR.IMediator mediator, DbContextOptions<SystemSettingsAggContext> options, IServiceProvider scope)
            : base(mediator, options, scope)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new UserProfileAccessMapping());
			builder.ApplyConfiguration(new UserMapping());
			builder.ApplyConfiguration(new SystemPanelSubItemMapping());
			builder.ApplyConfiguration(new SystemPanelMapping());
			builder.ApplyConfiguration(new SystemPanelGroupMapping());
			builder.ApplyConfiguration(new CargaTabelaMapping());
			builder.ApplyConfiguration(new SystemSettingsAggSettingsMapping());
		
			ApplyAdditionalMappings(builder);
			base.OnModelCreating(builder);
		}
		partial void ApplyAdditionalMappings(ModelBuilder modelBuilder);
	}
}
