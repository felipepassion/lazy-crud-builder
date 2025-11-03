using Lazy.Crud.Core.Api.Queries.Extensions;
using Lazy.Crud.CrossCuting.Infra.Utils.Extensions;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Lazy.Crud.Core.Api.Queries
{
    public class BaseQueryModel : IQueryModel
    {
        [JsonIgnore]
        public string? OrderBy { get; set; }
        [JsonIgnore]
        public string? Selector { get; set; }
        [JsonIgnore]
        public bool? OrderByDesc { get; set; }
        [JsonIgnore]
        public bool? GetOnlySummary { get; set; }
        [JsonIgnore]
        public bool IsOrSpecification { get; set; }
    }
}
