namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class TagController : SharedController<ITagService>
{
    public TagController(ITagService service) : base(service) { }

    [HttpGet("list")]
    public IActionResult Read() => ResponseResult(_service.FindAll());
}
