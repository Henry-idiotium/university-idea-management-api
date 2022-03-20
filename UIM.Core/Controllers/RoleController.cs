namespace UIM.Core.Controllers.Shared;

[JwtAuthorize]
public class RoleController : UimController<IRoleService>
{
    public RoleController(IRoleService service) : base(service) { }

    [HttpGet("[controller]s")]
    public IActionResult Read() => ResponseResult(_service.FindAll());

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var roleId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(roleId);
        return ResponseResult(result);
    }
}