namespace UIM.Core.Controllers;

public class DepartmentController : CrudController<IDepartmentService,
    CreateDepartmentRequest,
    UpdateDepartmentRequest,
    DepartmentDetailsResponse>
{
    public DepartmentController(IDepartmentService service) : base(service)
    {

    }

    [JwtAuthorize]
    public override async Task<IActionResult> Get([FromQuery] SieveModel request) => await base.Get(request);

    [JwtAuthorize]
    public override async Task<IActionResult> Get(string id) => await base.Get(id);
}