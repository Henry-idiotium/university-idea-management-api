namespace Namespace;

[ApiController]
[Route("api/idea-management")]
public class IdeaController : AdminController<IIdeaService,
    CreateIdeaRequest,
    UpdateIdeaRequest,
    IdeaDetailsResponse>
{
    public IdeaController(IIdeaService ideaService) : base(ideaService)
    {

    }
}