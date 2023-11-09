using LazyCrudBuilder.Core.Application.DTO.Extensions;
using LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Requests;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrudBuilder.Users.Application.Aggregates.UsersAgg.AppServices;

public partial class UserAppService
{
    public async Task<UserDTO> GetUserByCustomFilder(string customFilter)
    {
        User meuUsuario = await this._userRepository.FindAsync(x => x.Name == customFilter);

        return meuUsuario.ProjectedAs<UserDTO>();
    }
}
