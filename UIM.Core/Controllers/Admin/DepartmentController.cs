namespace UIM.Core.Controllers.Admin;

public class DepartmentController : AdminController<IDepartmentService>
{
    public DepartmentController(IDepartmentService service) : base(service) { }

    [HttpPost("api/[controller]-management")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
    {
        await _service.CreateAsync(request);
        return ResponseResult();
    }

    [HttpDelete("api/[controller]-management/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        await _service.RemoveAsync(entityId);
        return ResponseResult();
    }

    [HttpGet("api/[controller]/list")]
    public IActionResult Read() => ResponseResult(_service.FindAll());

    [HttpGet("api/[controller]-management/list")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var result = await _service.FindAsync(request);
        return ResponseResult(result);
    }

    [HttpGet("api/[controller]-management/{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }

    [HttpPut("api/[controller]-management/{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateDepartmentRequest request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        request.Id = EncryptHelpers.DecodeBase64Url(id);

        await _service.EditAsync(request);
        return ResponseResult();
    }
}
