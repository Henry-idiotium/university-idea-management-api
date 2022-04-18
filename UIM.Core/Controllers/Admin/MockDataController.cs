/* namespace UIM.Core.Controllers.Admin;

[JwtAuthorize(RoleNames.Admin)]
[Route("api/[controller]")]
public class MockDataController : UimController
{
    private readonly IJwtService _jwtService;
    private readonly IIdeaService _ideaService;
    private readonly IUserService _userService;

    public MockDataController() { }

    [HttpPost("ideas")]
    public async Task<IActionResult> CreateIdeas(IEnumerable<CreateIdeaRequest> request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService1.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.UserId = userId;
        request.SubmissionId = EncryptHelpers.DecodeBase64Url(request.SubmissionId);

        await _service.CreateAsync(request);

        return ResponseResult();
    }
}
 */
