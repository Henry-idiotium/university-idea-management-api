namespace UIM.Core.Services.Interfaces;

public interface IDepartmentService
    : ICrudService<
        CreateDepartmentRequest,
        UpdateDepartmentRequest,
        DepartmentDetailsResponse>
{

}