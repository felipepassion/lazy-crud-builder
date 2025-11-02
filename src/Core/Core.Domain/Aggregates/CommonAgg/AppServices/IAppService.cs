namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.AppServices
{
    public interface IAppService<T>
        where T : class
    {
        Task GetListAsync(object request);
        Task GetAsync(object request);
        Task CreateAsync(object request);
        Task UpdateAsync(object request);
        Task DeleteAsync(object request);
    }
}
