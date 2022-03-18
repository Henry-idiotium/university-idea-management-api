namespace UIM.Core.Services.Interfaces;

public interface IUserService
    : IService<
        CreateUserRequest,
        UpdateUserRequest,
        UserDetailsResponse>
{
    Task AddToDepartmentAsync(AppUser user, string? department);
}