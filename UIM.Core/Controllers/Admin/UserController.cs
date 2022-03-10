using Microsoft.AspNetCore.Mvc;
using UIM.Core.Common;
using UIM.Core.Models.Dtos.User;
using UIM.Core.Services.Interfaces;

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