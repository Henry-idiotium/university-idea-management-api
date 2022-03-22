namespace UIM.Core.Controllers.Shared;

public class TagController : SharedController<ITagService>
{
    public TagController(ITagService service) : base(service) { }

    [HttpGet("[controller]s")]
    public IActionResult Read() => ResponseResult(_service.FindAll());
}