namespace UIM.Core.Controllers;

[JwtAuthorize]
[Route("api/[controller]-management")]
public class IdeaController : UimController<IIdeaService>
{
    private readonly IJwtService _jwtService;

    public IdeaController(IIdeaService ideaService,
        IJwtService jwtService)
        : base(ideaService)
    {
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIdeaRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var userId = GetUserIdFromToken();

        await _service.CreateAsync(userId, request);
        return ResponseResult();
    }

    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserIdFromToken();
        var entityId = EncryptHelpers.DecodeBase64Url(id);

        await _service.RemoveAsync(userId, entityId);
        return ResponseResult();
    }

    [HttpGet("[controller]s")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var result = await _service.FindAsync(request);
        return ResponseResult(result);
    }

    [HttpGet("[controller]/{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }

    [HttpPut("[controller]/{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateIdeaRequest request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var userId = GetUserIdFromToken();
        var entityId = EncryptHelpers.DecodeBase64Url(id);

        await _service.EditAsync(entityId, userId, request);
        return ResponseResult();
    }

    private string GetUserIdFromToken()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        return userId;
    }
}