namespace UIM.Core.Controllers.Shared;

public class RoleController : SharedController<IRoleService>
{
    public RoleController(IRoleService service) : base(service) { }

    [HttpGet]
    public IActionResult Read() => ResponseResult(_service.FindAll());
}