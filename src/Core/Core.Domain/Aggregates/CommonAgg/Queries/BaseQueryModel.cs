using MediatR;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.CrossCutting;
using System.Linq.Expressions;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries
{
    public abstract class BaseQueryModel<T> : IQueryModel<T>
        where T:IEntity
    {
        public abstract Expression<Func<T, bool>> GetFilter();
        public string? OrderBy { get; set; }
        public string? Selector { get; set; }
        public bool? OrderByDesc { get; set; }
        public bool? GetOnlySummary { get; set; }
        public bool? IsOrSpecification { get; set; }
        public IRequest<DomainResponse>? Command { get; set; }
    }
}
