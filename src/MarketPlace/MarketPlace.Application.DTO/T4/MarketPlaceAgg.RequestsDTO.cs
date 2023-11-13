

using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LazyCrud.Core.Application.DTO.Attributes;

namespace LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Requests 
{
public partial class ProdutoDTO : EntityDTO
	{
	    public  string Nome { get; set; }
	    public  int Estoque { get; set; }
	    public  int Preco { get; set; }
	    public  int CategoriaId { get; set; }
	}
public partial class MarketPlaceAggSettingsDTO : BaseAggregateSettingsDTO
	{
	    public  int UserId { get; set; }
	}
public partial class CarrinhoDTO : EntityDTO
	{
	    public  int ProdutoId { get; set; }
	    public  int UsuarioId { get; set; }
	    public  int Pagamento { get; set; }
	}
public partial class CategoriaprodutoDTO : EntityDTO
	{
	    public LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects.ImageFileInfoDTO Nome { get; set; } = new LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects.ImageFileInfoDTO();
	}
}
namespace LazyCrud.MarketPlace.Application.DTO.Aggregates.UsersAgg.Requests 
{
public partial class UserDTO : EntityDTO
	{
	}
}
