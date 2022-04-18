namespace UIM.Core.Services.Interfaces;

public interface IUserService
{
    Task AddToDepartmentAsync(AppUser user, string? department);
    Task CreateAsync(CreateUserRequest request);
    Task EditAsync(UpdateUserRequest request);
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<UserDetailsResponse> FindByEmailAsync(string email);
    Task<UserDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId);
}
