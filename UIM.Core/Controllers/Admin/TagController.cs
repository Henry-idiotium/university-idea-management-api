namespace UIM.Core.Controllers.Admin;

public class TagController : AdminController<ITagService>
{
    public TagController(ITagService service) : base(service) { }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        if (name == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        await _service.CreateAsync(name);
        return ResponseResult();
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await _service.RemoveAsync(name);
        return ResponseResult();
    }

    [HttpGet]
    public IActionResult Read() => ResponseResult(_service.FindAll());

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTagRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        await _service.EditAsync(request);
        return ResponseResult();
    }
}