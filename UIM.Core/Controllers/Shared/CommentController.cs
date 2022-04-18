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
        request.IdeaId = EncryptHelpers.DecodeBase64Url(request.IdeaId);

        await _service.CreateAsync(request);
        return ResponseResult();
    }

    [HttpGet("list/{ideaId}")]
    public async Task<IActionResult> Read(string ideaId, int? minItems)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var result = await _service.FindAllAsync(
            EncryptHelpers.DecodeBase64Url(ideaId),
            minItems ?? 0
        );
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

        await _service.RemoveAsync(EncryptHelpers.DecodeBase64Url(id), userId);
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
