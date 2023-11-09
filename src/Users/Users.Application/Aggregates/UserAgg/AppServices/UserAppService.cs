using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices;

public partial class UserAppService
{
    public async Task<UserDTO> GetUserByCustomFilder(string customFilter)
    {
        User meuUsuario = await this._userRepository.FindAsync(x => x.Name == customFilter);

        return meuUsuario.ProjectedAs<UserDTO>();
    }
}
