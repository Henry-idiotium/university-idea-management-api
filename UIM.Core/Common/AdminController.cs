namespace UIM.Core.Common;

[JwtAuthorize(RoleNames.Admin)]
[ApiController]
public abstract class AdminController<TService, TIdentity, TCreate, TUpdate, TDetails> : ControllerBase
    where TService : IService<TIdentity, TCreate, TUpdate, TDetails>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
    where TDetails : IResponse
    where TIdentity : IConvertible
{
    protected TService _service;

    public AdminController(TService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TCreate request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        await _service.CreateAsync(request);
        return new ActionResponse();
    }

    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> Delete(TIdentity id)
    {
        var entityId = DecodeGenericIdentity(id);
        await _service.RemoveAsync(entityId);
        return new ActionResponse();
    }

    [HttpPut("[controller]/{id}")]
    public async Task<IActionResult> Edit([FromBody] TUpdate request, TIdentity id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var entityId = DecodeGenericIdentity(id);

        await _service.EditAsync(entityId, request);
        return new ActionResponse();
    }

    [HttpGet("[controller]s")]
    public virtual async Task<IActionResult> Get([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var result = await _service.FindAsync(request);
        return new ActionResponse(result);
    }

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Get(TIdentity id)
    {
        var entityId = DecodeGenericIdentity(id);
        var result = await _service.FindByIdAsync(entityId);
        return new ActionResponse(result);
    }

    private static TIdentity DecodeGenericIdentity(TIdentity id)
    {
        if (id is string)
        {
            id = (TIdentity)Convert.ChangeType(
                value: EncryptHelpers.DecodeBase64Url(id.ToString()),
                conversionType: typeof(TIdentity));
        }
        return id;
    }
}