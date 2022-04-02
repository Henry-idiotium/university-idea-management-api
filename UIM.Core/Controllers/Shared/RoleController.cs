namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class RoleController : SharedController<IRoleService>
{
    public RoleController(IRoleService service) : base(service) { }

    [HttpGet("list")]
    public IActionResult Read() => ResponseResult(_service.FindAll());
}
