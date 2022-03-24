namespace UIM.Core.Controllers.Shared;

public class TagController : SharedController<ITagService>
{
    public TagController(ITagService service) : base(service) { }

    [HttpGet]
    public IActionResult Read() => ResponseResult(_service.FindAll());
}