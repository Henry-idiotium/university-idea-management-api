namespace UIM.Core.Controllers;

[JwtAuthorize(RoleNames.Admin)]
[Route("api/[controller]-management")]
public class DepartmentController : UimController<IDepartmentService>
{
    public DepartmentController(IDepartmentService service) : base(service) { }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        await _service.CreateAsync(request);
        return ResponseResult();
    }

    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        await _service.RemoveAsync(entityId);
        return ResponseResult();
    }

    [JwtAuthorize]
    [HttpGet("[controller]s")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var result = await _service.FindAsync(request);
        return ResponseResult(result);
    }

    [JwtAuthorize]
    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }

    [HttpPut("[controller]/{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateDepartmentRequest request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var entityId = EncryptHelpers.DecodeBase64Url(id);

        await _service.EditAsync(entityId, request);
        return ResponseResult();
    }
}