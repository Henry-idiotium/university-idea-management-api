namespace UIM.Core.Controllers.Shared;

[Route("api")]
public class RoleController : SharedController<IRoleService>
{
    public RoleController(IRoleService service) : base(service)
    {

    }

    [HttpGet("[controller]s")]
    public IActionResult Get() => ResponseResult(_service.FindAll());

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var roleId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(roleId);
        return ResponseResult(result);
    }
}