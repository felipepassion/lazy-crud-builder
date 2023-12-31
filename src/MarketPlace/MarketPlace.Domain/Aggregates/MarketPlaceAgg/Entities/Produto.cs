using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public partial class Produto : Entity
    {
        public string Nome { get; set; }
    
        public int Estoque { get; set; }
    
        public int Preco { get; set; }
    
        public int CategoriaId { get; set; }
    }
}
