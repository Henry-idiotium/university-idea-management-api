namespace UIM.Core.Services.Interfaces;

public interface IDepartmentService
{
    Task CreateAsync(CreateDepartmentRequest request);
    Task EditAsync(UpdateDepartmentRequest request);
    IEnumerable<SimpleDepartmentResponse> FindAll();
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<DepartmentDetailsResponse> FindByIdAsync(string entityId);
    Task<DepartmentDetailsResponse> FindByNameAsync(string name);
    Task RemoveAsync(string entityId);
}