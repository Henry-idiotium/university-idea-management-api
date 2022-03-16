namespace UIM.Core.Common;

[JwtAuthorize]
public abstract class SharedController<TService> : UimController
{
    protected TService _service;

    public SharedController(TService service) => _service = service;
}