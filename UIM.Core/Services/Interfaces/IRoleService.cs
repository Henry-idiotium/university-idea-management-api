namespace UIM.Core.Services.Interfaces;

public interface IRoleService
{
    IEnumerable<RoleDetailsResponse> FindAll();
    Task<RoleDetailsResponse> FindByIdAsync(string entityId);
    Task<RoleDetailsResponse> FindByNameAsync(string name);
}