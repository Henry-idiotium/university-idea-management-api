namespace UIM.Core.Services.Interfaces;

public interface IUserService
    : IService<string,
        CreateUserRequest,
        UpdateUserRequest,
        UserDetailsResponse>
{

}