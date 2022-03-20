namespace UIM.Core.Services.Interfaces;

public interface IUserService
    : IReadService<UserDetailsResponse>
    , IModifyService<CreateUserRequest, UpdateUserRequest>
{
    Task AddToDepartmentAsync(AppUser user, string? department);
}