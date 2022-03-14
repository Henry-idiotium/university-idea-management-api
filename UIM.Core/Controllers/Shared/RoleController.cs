namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class RoleController : SharedController<IRoleService>
{
    public RoleController(IRoleService service) : base(service)
    {

    }

    [HttpGet("[controller]s")]
    public async Task<IActionResult> Get([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var result = await _service.FindAsync(request);
        return Ok(new GenericResponse(result));
    }

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _service.FindByIdAsync(EncryptHelpers.DecodeBase64Url(id));
        return Ok(new GenericResponse(result));
    }
}