namespace UIM.Core.Common;

[JwtAuthorize(RoleNames.Admin)]
[Route("api/[controller]-management")]
public abstract class CrudController<TService, TCreate, TUpdate, TDetails> : UimController
    where TService : IService<TCreate, TUpdate, TDetails>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
    where TDetails : IResponse
{
    protected TService _service;

    public CrudController(TService service) => _service = service;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TCreate request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        await _service.CreateAsync(request);
        return ResponseResult();
    }

    [HttpDelete("[controller]/{id}")]
    public virtual async Task<IActionResult> Delete(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        await _service.RemoveAsync(entityId);
        return ResponseResult();
    }

    [HttpPut("[controller]/{id}")]
    public virtual async Task<IActionResult> Edit([FromBody] TUpdate request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var entityId = EncryptHelpers.DecodeBase64Url(id);

        await _service.EditAsync(entityId, request);
        return ResponseResult();
    }

    [HttpGet("[controller]s")]
    public virtual async Task<IActionResult> Get([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var result = await _service.FindAsync(request);
        return ResponseResult(result);
    }

    [HttpGet("[controller]/{id}")]
    public virtual async Task<IActionResult> Get(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }
}