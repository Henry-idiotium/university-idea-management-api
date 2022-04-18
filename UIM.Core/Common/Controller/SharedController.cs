namespace UIM.Core.Common.Controller;

[JwtAuthorize]
[Route("")]
public abstract class SharedController<TService> : UimController
{
    protected TService _service;
    protected SharedController(TService service) => _service = service;
}
