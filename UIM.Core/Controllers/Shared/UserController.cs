namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class UserController : SharedController<IUserService>
{
    public UserController(IUserService service) : base(service) { }

    [HttpGet("{email}")]
    public async Task<IActionResult> Read(string email)
    {
        var result = await _service.FindByEmailAsync(email);
        return ResponseResult(result);
    }
}
