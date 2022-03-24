namespace UIM.Core.Common.Controller;

[JwtAuthorize]
[Route("api/[controller]")]
public abstract class SharedController<TService> : UimController
{
    protected TService _service;
    protected SharedController(TService service) => _service = service;
}