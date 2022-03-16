namespace UIM.Core.Services.Interfaces;

public interface IDepartmentService
    : IService<
        CreateDepartmentRequest,
        UpdateDepartmentRequest,
        DepartmentDetailsResponse>
{

}