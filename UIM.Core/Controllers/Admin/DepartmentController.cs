using Microsoft.AspNetCore.Mvc;
using UIM.Core.Common;
using UIM.Core.Models.Dtos.Department;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Controllers
{
    [ApiController]
    [Route("api/department-management")]
    public class DepartmentController : UimController<IDepartmentService, int,
        CreateDepartmentRequest,
        UpdateDepartmentRequest,
        DepartmentDetailsResponse>
    {
        public DepartmentController(IDepartmentService service) : base(service)
        {

        }
    }
}