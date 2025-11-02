using System.Linq.Expressions;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Repositories
{
    public interface IMongoRepository<T> 
        where T : class
    {
        Task ClearDatabase();
        Task InsertAsync(T entity);
        Task InsertRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task DeleteManyWithAuditAsync(Expression<Func<T, bool>> expression);
        Task UpdateLogRangeAsync(List<T> entities);
        Task<T> FindAsync(Expression<Func<T, bool>> query);
        Task<long> CountAsync();
        Task RenameCollection(string newName, string oldName);
        Task DropCollection(string collectionName);
        Task Duoplicate(string newName);
        Task<IEnumerable<K>> SelectAllAsync<K>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, K>> selector,
                Expression<Func<T, object>>[]? orderBy = null,
            bool ascending = true,
            int? skip = null,
            int? take = null,
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
    }
}
