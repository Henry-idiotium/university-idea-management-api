namespace UIM.Core.Controllers.Shared;

public class TagController : SharedController<ITagService>
{
    public TagController(ITagService service) : base(service) { }

    [HttpGet("api/[controller]s")]
    public IActionResult Read() => ResponseResult(_service.FindAll());
}
