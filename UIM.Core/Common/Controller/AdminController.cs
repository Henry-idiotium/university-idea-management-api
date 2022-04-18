namespace UIM.Core.Common.Controller;

[JwtAuthorize(RoleNames.Admin, RoleNames.Manager)]
[Route("api/[controller]-management")]
public abstract class AdminController<TService> : UimController
{
    protected TService _service;
    protected AdminController(TService service) => _service = service;
}
