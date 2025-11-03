using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Core.Domain.CrossCutting;

namespace Lazy.Crud.Core.Domain.Seedwork
{
    public interface IUnitOfWork
    {
        Task<bool> ExecuteNpCommand(string rawText);
        bool Commit(object? data = null);
        void ResolveAttaches<T>(T entity) where T : Entity;
        void ResolveAttachesOnUpdate<T, K>(T entity, K entityDTO) where T : Entity, new() where K : Entity, new();
        Task<DomainResponse> CommitAsync(object? data = null);
        bool IsEmpty<T>(T entity) where T : class;
    }
}
