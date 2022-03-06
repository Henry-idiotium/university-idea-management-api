using UIM.Core.Common;
using UIM.Core.Models.Dtos.Department;

namespace UIM.Core.Services.Interfaces
{
    public interface IDepartmentService
        : IService<int,
            CreateDepartmentRequest,
            UpdateDepartmentRequest,
            DepartmentDetailsResponse>
    {

    }
}