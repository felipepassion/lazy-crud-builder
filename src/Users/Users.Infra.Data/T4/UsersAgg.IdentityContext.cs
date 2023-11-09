
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities; 
using LazyCrud.Users.Infra.Data.Aggregates.UsersAgg.Mappings; 
using LazyCrud.Users.Domain.Aggregates.SystemSettingsAgg.Entities; 
using LazyCrud.Users.Infra.Data.Aggregates.SystemSettingsAgg.Mappings; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LazyCrud.Core.Infra.Data.Contexts;

namespace LazyCrud.Users.Infra.Data.Context
{
	public partial class UsersAggContext : BaseContext
	{
		public DbSet<UserProfileAccess> UserProfileAccess { get; set; }
		public DbSet<UserCurrentAccessSelected> UserCurrentAccessSelected { get; set; }
		public DbSet<UserProfileList> UserProfileList { get; set; }
		public DbSet<UserProfile> UserProfile { get; set; }
		public DbSet<UsersAggSettings> UsersAggSettings { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<UserContact> UserContact { get; set; }
		public DbSet<SystemPanelSubItem> SystemPanelSubItem { get; set; }
		public DbSet<SystemPanel> SystemPanel { get; set; }
		public DbSet<SystemPanelGroup> SystemPanelGroup { get; set; }

		public UsersAggContext (MediatR.IMediator mediator, DbContextOptions<UsersAggContext> options, IServiceProvider scope)
            : base(mediator, options, scope)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new UserProfileAccessMapping());
			builder.ApplyConfiguration(new UserCurrentAccessSelectedMapping());
			builder.ApplyConfiguration(new UserProfileListMapping());
			builder.ApplyConfiguration(new UserProfileMapping());
			builder.ApplyConfiguration(new UsersAggSettingsMapping());
			builder.ApplyConfiguration(new UserMapping());
			builder.ApplyConfiguration(new UserContactMapping());
			builder.ApplyConfiguration(new SystemPanelSubItemMapping());
			builder.ApplyConfiguration(new SystemPanelMapping());
			builder.ApplyConfiguration(new SystemPanelGroupMapping());
		
			ApplyAdditionalMappings(builder);
			base.OnModelCreating(builder);
		}
		partial void ApplyAdditionalMappings(ModelBuilder modelBuilder);
	}
}
