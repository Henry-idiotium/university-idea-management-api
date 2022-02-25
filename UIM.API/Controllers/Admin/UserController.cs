using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.Model.Dtos.Common;
using UIM.Model.Dtos.User;

namespace UIM.API.Controllers
{
    [ApiController]
    [Route("api/user-management")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _userService.CreateAsync(request);
            return Ok(new GenericResponse(SuccessResponseMessages.UserRegistered));
        }

        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(string id, [FromQuery] UpdateUserInfoRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _userService.UpdateInfoAsync(id, request);
            return Ok(new GenericResponse());
        }

        [HttpPut("{id}/profile/password")]
        public async Task<IActionResult> UpdatePassword(string id, [FromQuery] UpdateUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _userService.UpdatePasswordAsync(id, request);
            return Ok(new GenericResponse());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _userService.DeleteAsync(id);
            return Ok(new GenericResponse());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var user = await _userService.GetByIdAsync(id);
            return Ok(new GenericResponse(user));
        }

        [HttpGet("users")]
        public async Task<IActionResult> Get([FromQuery] SieveModel request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var users = await _userService.GetUsersAsync(request);
            return Ok(new GenericResponse(users));
        }
    }
}