namespace UIM.Core.Controllers;

[ApiController]
[Route("api/department-management")]
public class DepartmentController : AdminController<IDepartmentService,
    CreateDepartmentRequest,
    UpdateDepartmentRequest,
    DepartmentDetailsResponse>
{
    public DepartmentController(IDepartmentService service) : base(service)
    {

    }
}