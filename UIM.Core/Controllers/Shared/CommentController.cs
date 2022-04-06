namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class CommentController : SharedController<ICommentService>
{
    private readonly IJwtService _jwtService;

    public CommentController(ICommentService service, IJwtService jwtService) : base(service)
    {
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
    {
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

    [HttpGet("table/list/{ideaId}")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request, string ideaId)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var decodedIdeaId = EncryptHelpers.DecodeBase64Url(ideaId);
        var result = await _service.FindAsync(decodedIdeaId, request);
        return ResponseResult(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var entityId = EncryptHelpers.DecodeBase64Url(id);
        await _service.RemoveAsync(entityId, userId);
        return ResponseResult();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateCommentRequest request, string id)
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
        request.Id = EncryptHelpers.DecodeBase64Url(id);

        await _service.EditAsync(request);
        return ResponseResult();
    }
}
