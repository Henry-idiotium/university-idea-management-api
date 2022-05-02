namespace UIM.Core.Controllers.Admin;

[JwtAuthorize(RoleNames.Admin, RoleNames.Manager)]
[Route("api/[controller]-management")]
public class MockController : UimController
{
    private readonly IUserService _userService;
    private readonly IIdeaService _ideaService;
    private readonly ISubmissionService _submissionService;

    public MockController(
        IUserService userService,
        IIdeaService ideaService,
        ISubmissionService submissionService
    )
    {
        _userService = userService;
        _ideaService = ideaService;
        _submissionService = submissionService;
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUsers([FromBody] List<CreateUserRequest> request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        foreach (var userRequest in request)
            await _userService.MockCreateAsync(userRequest, "mockuser");

        return ResponseResult();
    }

    [HttpPost("submissions")]
    public async Task<IActionResult> CreateSubmissions(
        [FromBody] List<CreateSubmissionRequest> request
    )
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        foreach (var subRequest in request)
            await _submissionService.MockCreateAsync(subRequest);

        return ResponseResult();
    }
}
