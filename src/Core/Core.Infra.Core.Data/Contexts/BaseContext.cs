using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Events;
using Niu.Nutri.Core.Domain.CrossCutting;
using Niu.Nutri.Core.Domain.Seedwork;
using Niu.Nutri.Core.Infra.Data.Extensions;
using Niu.Nutri.CrossCutting.Infra.Log.Contexts;
using Npgsql;

namespace Niu.Nutri.Core.Infra.Data.Contexts
{
    public class BaseContext : DbContext, IUnitOfWork, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public bool IsEmpty<T>(T entity) where T : class => this.Find<T>() == null;

        IMediator _mediator;
        private readonly IServiceProvider _scope;

        public BaseContext(IMediator mediator, DbContextOptions options, IServiceProvider scope)
            : base(options)
        {
            _mediator = mediator;
            _scope = scope;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataProtectionKey>().Metadata.SetIsTableExcludedFromMigrations(true);
            base.OnModelCreating(modelBuilder);
        }

        public bool Commit(object? data = null)
        {
            return CommitAsync(data).Result?.Success == true;
        }

        public async Task<DomainResponse> CommitAsync(object? data)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            try
            {
                await SaveChangesAsync();

                await _mediator.PublishDomainEvents(this).ConfigureAwait(false);

                return new DomainResponse(data);
            }
            catch (Exception ex)
            {
                var context = _scope.GetRequiredService<ILogRequestContext>();
                await _mediator.Publish(new ErrorEvent(context, ex, "DATABASE COMMIT ERROR", data));
                return new DomainResponse(ex);
            }
        }

        public async Task<bool> ExecuteNpCommand(string rawText)
        {
            try
            {
                await this.Database.ExecuteSqlRawAsync(new NpgsqlCommand(rawText).CommandText);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public void ResolveAttaches<T>(T entity)
            where T : Entity
        {
        }

        public void ResolveAttachesOnUpdate<T, K>(T entity, K entityDTO)
            where T : Entity, new()
            where K : Entity, new()
        {
        }
    }
}



