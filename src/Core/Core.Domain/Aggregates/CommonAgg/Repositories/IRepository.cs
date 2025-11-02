using Niu.Nutri.Core.Domain.Seedwork;
using System.Linq.Expressions;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Repositories
{
    public interface IRepository<T> : IDisposable
        where T : class, new()
    {
        void Add(T entity);
        void Attach<K>(K entity);
        void Dettach<K>(K entity);
        void Added<K>(K entity);
        void AddRange(IEnumerable<T> entity);
        void DeleteRange(IEnumerable<T> entity);
        void Delete(T entity);
        void Update(T entity);
        Task<T> GetRandom();
        Task<List<T>> GetRandomList(int max = 10);
        bool IsEmpty();
        bool Any(Expression<Func<T, bool>>? filter = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> Exists(Expression<Func<T, bool>> spec);
        Task<T> FindAsync(Expression<Func<T, bool>> spec, bool includeAll = true);
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<T> FindAsync(
            Expression<Func<T, bool>> filter,
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
        Task<bool> ExecuteCommandAsync(string command);
        Task<K> SelectAsync<K>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, K>> selector,
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
        Task<K> FindAsync<K>(
            Expression<Func<T, K>> selector,
            Expression<Func<T, bool>> filter, 
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
        Task<IEnumerable<T>> FindAllAsync(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>>[]? orderBy = null,
            bool ascending = true,
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
        Task<IEnumerable<K>> FindAllAsync<K>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, K>> selector,
            Expression<Func<T, object>>[]? orderBy = null,
            bool ascending = true,
            int? skip = null,
            int? take = null,
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
        Task<IEnumerable<K>> SelectAllAsync<K>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, K>> selector,
                Expression<Func<T, object>>[]? orderBy = null,
            bool ascending = true,
            int? skip = null,
            int? take = null,
            bool includeAll = true,
            params Expression<Func<T, object>>[] include);
        Task<IEnumerable<IGrouping<object, K>>> GroupByAsync<K>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, K>> selector,
            Expression<Func<K, object>> groupBy,
            Expression<Func<T, object>>[]? orderBy,
            bool ascending = true,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[]? include);

        public IUnitOfWork UnitOfWork { get; }
    }
}
