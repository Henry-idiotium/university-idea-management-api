namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService) => _roleService = roleService;

    [HttpGet("[controller]s")]
    public async Task<IActionResult> Get([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var result = await _roleService.FindAsync(request);
        return Ok(new GenericResponse(result));
    }

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _roleService.FindByIdAsync(EncryptHelpers.DecodeBase64Url(id));
        return Ok(new GenericResponse(result));
    }
}