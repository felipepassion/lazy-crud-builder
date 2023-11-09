using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.AppServices
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
