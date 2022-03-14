namespace UIM.Core.Services.Interfaces;

public interface IRoleService
    : IService<string, ICreateRequest, IUpdateRequest, RoleDetailsResponse>
{
    Task<RoleDetailsResponse> FindByNameAsync(string name);
}