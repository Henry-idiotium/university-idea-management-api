namespace Namespace;

[ApiController]
[Route("api/idea-management")]
public class IdeaController : AdminController<IIdeaService, string,
    CreateIdeaRequest,
    UpdateIdeaRequest,
    IdeaDetailsResponse>
{
    public IdeaController(IIdeaService ideaService) : base(ideaService)
    {

    }
}