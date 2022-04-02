namespace UIM.Core.Controllers.Shared;

public class SubmissionController : SharedController<ISubmissionService>
{
    public SubmissionController(ISubmissionService service) : base(service) { }

    [HttpGet("api/[controller]s")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var result = await _service.FindAsync(request);
        return ResponseResult(result);
    }

    [HttpGet("api/[controller]/{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }
}
