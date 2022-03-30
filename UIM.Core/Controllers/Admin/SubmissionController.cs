namespace UIM.Core.Controllers.Admin;

[Route("api/[controller]-management")]
public class SubmissionController : AdminController<ISubmissionService>
{
    private readonly IJwtService _jwtService;

    public SubmissionController(ISubmissionService service, IJwtService jwtService) : base(service)
    {
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubmissionRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.UserId = userId;

        await _service.CreateAsync(request);
        return ResponseResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        await _service.RemoveAsync(entityId);
        return ResponseResult();
    }

    [HttpGet]
    public async Task<IActionResult> Read([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var result = await _service.FindAsync(request);
        return ResponseResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateSubmissionRequest request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.Id = EncryptHelpers.DecodeBase64Url(id);
        request.UserId = userId;

        await _service.EditAsync(request);
        return ResponseResult();
    }
}