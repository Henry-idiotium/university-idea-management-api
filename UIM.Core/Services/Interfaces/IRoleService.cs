namespace UIM.Core.Services.Interfaces;

public interface IRoleService
{
    // Task<SieveResponse> FindAsync(SieveModel model);
    IEnumerable<RoleDetailsResponse> FindAll();
    Task<RoleDetailsResponse> FindByIdAsync(string entityId);
    Task<RoleDetailsResponse> FindByNameAsync(string name);
}