using System.ComponentModel;
using Lazy.Crud.Builder.Application.DTO.Attributes;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Builder.Domain.Attributes.T4;

namespace Lazy.Crud.{Aggregate}.Domain.Aggregates.{Aggregate}Agg.Entities
{
    /// <summary>A category grouping products. Second entity of the reference aggregate.</summary>
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class ProductCategory : Entity
    {
        [RequiredT4, DisplayOnList(1), DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Sort order")]
        public int SortOrder { get; set; }
    }
}
