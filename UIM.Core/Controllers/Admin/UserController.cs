using Microsoft.AspNetCore.Mvc;
using UIM.Core.Services.Interfaces;
using UIM.Core.Models.Dtos.User;
using UIM.Core.Common;

namespace UIM.Core.Controllers
{
    [ApiController]
    [Route("api/user-management")]
    public class UserController : UimController<IUserService, string,
        CreateUserRequest,
        UpdateUserRequest,
        UserDetailsResponse>
    {
        public UserController(IUserService service) : base(service)
        {

        }
    }
}