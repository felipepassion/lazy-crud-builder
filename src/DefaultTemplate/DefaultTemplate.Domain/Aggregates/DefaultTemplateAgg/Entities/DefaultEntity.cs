using Lazy.Crud.Core.Application.DTO.Attributes;
using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Core.Domain.Attributes.T4;

namespace Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public partial class DefaultEntity : Entity
    {
    }
}
