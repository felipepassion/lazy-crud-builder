using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lazy.Crud.Builder.Application.DTO.Attributes;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Builder.Domain.Attributes.T4;
using Lazy.Crud.{Aggregate}.Enumerations;

namespace Lazy.Crud.{Aggregate}.Domain.Aggregates.{Aggregate}Agg.Entities
{
    /// <summary>A product sold by the store. Reference aggregate root for the Lazy CRUD golden output.</summary>
    [AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class Product : Entity
    {
        [RequiredT4, Unique, DisplayOnList(1), DisplayName("SKU")]
        public string Sku { get; set; }

        [RequiredT4, DisplayOnList(2), DisplayName("Name")]
        public string Name { get; set; }

        [DisplayOnList(2), DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Stock")]
        public int Stock { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Available from")]
        public DateTime? AvailableFrom { get; set; }

        [DisplayName("Supplier")]
        public Guid SupplierId { get; set; }

        [DisplayOnList(3), DisplayName("Status")]
        public ProductStatus Status { get; set; }
    }
}
