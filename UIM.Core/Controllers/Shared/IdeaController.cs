using UIM.Core.Models.Dtos.Like;

namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class IdeaController : SharedController<IIdeaService>
{
    private readonly IJwtService _jwtService;

    public IdeaController(IIdeaService ideaService, IJwtService jwtService) : base(ideaService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("like/{ideaId}")]
    public async Task<IActionResult> AddLike(CreateLikeRequest request, string ideaId)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.IdeaId = EncryptHelpers.DecodeBase64Url(ideaId);
        request.UserId = userId;

        var response = await _service.AddLikenessAsync(request);
        return ResponseResult(response);
    }

    [HttpPost("view/{ideaId}")]
    public async Task<IActionResult> AddView(string ideaId)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        await _service.AddViewAsync(
            new CreateViewRequest
            {
                UserId = userId,
                IdeaId = EncryptHelpers.DecodeBase64Url(ideaId),
            }
        );
        return ResponseResult();
    }

    [HttpPut("like/{ideaId}")]
    public async Task<IActionResult> UpdateLike(CreateLikeRequest request, string ideaId)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.IdeaId = EncryptHelpers.DecodeBase64Url(ideaId);
        request.UserId = userId;

        var response = await _service.UpdateLikenessAsync(request);
        return ResponseResult(response);
    }

    [HttpDelete("like/{ideaId}")]
    public async Task<IActionResult> RemoveLike(string ideaId)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        await _service.DeleteLikenessAsync(userId, EncryptHelpers.DecodeBase64Url(ideaId));

        return ResponseResult();
    }

    [HttpPost]
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
        request.SubmissionId = EncryptHelpers.DecodeBase64Url(request.SubmissionId);

        var response = await _service.CreateAsync(request);
        return ResponseResult(response);
    }

    [HttpDelete("{id}")]
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

    [HttpGet("table/list")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request, string? submissionId)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var result = await _service.FindAsync(
            EncryptHelpers.DecodeBase64Url(submissionId),
            userId,
            request
        );
        return ResponseResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId, userId);
        return ResponseResult(result);
    }

    [HttpPut("{id}")]
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

        request.Id = EncryptHelpers.DecodeBase64Url(id);
        request.SubmissionId = EncryptHelpers.DecodeBase64Url(request.SubmissionId);
        request.UserId = userId;

        await _service.EditAsync(request);
        return ResponseResult();
    }
}
