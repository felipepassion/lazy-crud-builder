using Niu.Nutri.Core.Application.DTO.Attributes;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;
using Niu.Nutri.Core.Domain.Attributes.T4;

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public partial class DefaultEntity : Entity
    {
    }
}
