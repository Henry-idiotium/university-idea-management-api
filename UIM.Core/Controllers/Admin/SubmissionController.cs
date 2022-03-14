namespace UIM.Core.Controllers.Admin;

[ApiController]
[Route("api/submission-management")]
public class SubmissionController : AdminController<ISubmissionService, string,
    CreateSubmissionRequest,
    UpdateSubmissionRequest,
    SubmissionDetailsResponse>
{
    public SubmissionController(ISubmissionService service) : base(service)
    {
    }

    [HttpPost("[controller]/idea")]
    public async Task<IActionResult> AddIdea([FromBody] AddIdeaRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        await _service.AddIdeaToSubmissionAsync(request);
        return Ok(new GenericResponse());
    }
}