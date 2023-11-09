using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.CrossCutting;
using MediatR;
using System.Linq.Expressions;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries
{
    public interface IQueryModel<E>
        where E : IEntity
    {
        public Expression<Func<E, bool>> GetFilter();
        public string Selector { get; set; }
        public string OrderBy { get; set; }
        public bool? OrderByDesc { get; set; }
        public bool? GetOnlySummary { get; set; }
        public bool? IsOrSpecification { get; set; }
        public IRequest<DomainResponse> Command { get; set; }
    }
}
