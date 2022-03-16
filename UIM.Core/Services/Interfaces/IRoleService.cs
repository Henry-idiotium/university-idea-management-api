namespace UIM.Core.Services.Interfaces;

public interface IRoleService
    : IService<ICreateRequest, IUpdateRequest, RoleDetailsResponse>
{
    Task<RoleDetailsResponse> FindByNameAsync(string name);
}