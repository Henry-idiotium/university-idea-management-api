namespace UIM.Core.Controllers;

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