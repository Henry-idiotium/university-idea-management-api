namespace UIM.Core.Controllers;

[ApiController]
[Route("api/user-management")]
public class UserController : AdminController<IUserService,
    CreateUserRequest,
    UpdateUserRequest,
    UserDetailsResponse>
{
    public UserController(IUserService service) : base(service)
    {

    }
}