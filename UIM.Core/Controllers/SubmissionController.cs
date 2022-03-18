namespace UIM.Core.Controllers.Admin;

public class SubmissionController : CrudController<ISubmissionService,
    CreateSubmissionRequest,
    UpdateSubmissionRequest,
    SubmissionDetailsResponse>
{
    public SubmissionController(ISubmissionService service) : base(service)
    {
    }

    [JwtAuthorize]
    public override async Task<IActionResult> Get([FromQuery] SieveModel request) => await base.Get(request);

    [JwtAuthorize]
    public override async Task<IActionResult> Get(string id) => await base.Get(id);

    [HttpPut("managed-idea")]
    public async Task<IActionResult> AddIdea([FromBody] AddIdeaRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        await _service.AddIdeaAsync(request);
        return ResponseResult();
    }
}