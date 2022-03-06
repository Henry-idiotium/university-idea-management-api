using UIM.Core.Common;
using UIM.Core.Models.Dtos.User;

namespace UIM.Core.Services.Interfaces
{
    public interface IUserService
        : IService<string,
            CreateUserRequest,
            UpdateUserRequest,
            UserDetailsResponse>
    {

    }
}