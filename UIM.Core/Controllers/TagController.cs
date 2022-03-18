namespace UIM.Core.Controllers;

public class TagController : CrudController<ITagService,
    CreateTagRequest,
    UpdateTagRequest,
    TagDetailsResponse>
{
    public TagController(ITagService service) : base(service)
    {

    }

    [JwtAuthorize]
    public override async Task<IActionResult> Get([FromQuery] SieveModel request) => await base.Get(request);

    [JwtAuthorize]
    public override async Task<IActionResult> Get(string id) => await base.Get(id);
}