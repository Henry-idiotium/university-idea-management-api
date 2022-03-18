namespace Namespace;

[JwtAuthorize]
public class IdeaController : CrudController<IIdeaService,
    CreateIdeaRequest,
    UpdateIdeaRequest,
    IdeaDetailsResponse>
{
    public IdeaController(IIdeaService ideaService) : base(ideaService)
    {

    }
}