namespace UIM.Core.Controllers;

public class UserController : CrudController<IUserService,
    CreateUserRequest,
    UpdateUserRequest,
    UserDetailsResponse>
{
    public UserController(IUserService service) : base(service)
    {

    }
}