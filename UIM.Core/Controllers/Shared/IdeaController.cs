namespace UIM.Core.Controllers.Shared;

public class IdeaController : SharedController<IIdeaService>
{
    private readonly IJwtService _jwtService;

    public IdeaController(IIdeaService ideaService, IJwtService jwtService) : base(ideaService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("api/[controller]")]
    public async Task<IActionResult> Create([FromBody] CreateIdeaRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.UserId = userId;

        await _service.CreateAsync(request);
        return ResponseResult();
    }

    [HttpDelete("api/[controller]/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var entityId = EncryptHelpers.DecodeBase64Url(id);

        await _service.RemoveAsync(userId, entityId);
        return ResponseResult();
    }

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

    [HttpPut("api/[controller]/{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateIdeaRequest request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var entityId = EncryptHelpers.DecodeBase64Url(id);

        request.Id = entityId;
        request.UserId = userId;

        await _service.EditAsync(request);
        return ResponseResult();
    }
}
