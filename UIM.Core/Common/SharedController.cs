namespace UIM.Core.Common;

[JwtAuthorize]
[ApiController]
public abstract class SharedController<TService> : ControllerBase
{
    protected TService _service;

    public SharedController(TService service) => _service = service;
}