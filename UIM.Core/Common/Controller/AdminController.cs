namespace UIM.Core.Common.Controller;

[JwtAuthorize(RoleNames.Admin)]
public abstract class AdminController<TService> : UimController
{
    protected TService _service;
    protected AdminController(TService service) => _service = service;
}