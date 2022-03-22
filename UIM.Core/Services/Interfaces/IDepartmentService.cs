namespace UIM.Core.Services.Interfaces;

public interface IDepartmentService
{
    Task CreateAsync(string name);
    Task EditAsync(UpdateDepartmentRequest request);
    IEnumerable<DepartmentDetailsResponse> FindAll();
    Task<DepartmentDetailsResponse> FindByIdAsync(string entityId);
    Task<DepartmentDetailsResponse> FindByNameAsync(string name);
    Task RemoveAsync(string name);
}