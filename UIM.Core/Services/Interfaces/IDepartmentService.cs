namespace UIM.Core.Services.Interfaces;

public interface IDepartmentService
    : IService<int,
        CreateDepartmentRequest,
        UpdateDepartmentRequest,
        DepartmentDetailsResponse>
{

}