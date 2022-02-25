using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UIM.BAL.Services.Interfaces;
using UIM.Common.ResponseMessages;
using UIM.Model.Dtos.Auth;
using UIM.Model.Dtos.Common;
using UIM.Model.Dtos.Token;

namespace UIM.Common.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLogin(ExternalAuthRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            return Ok(new GenericResponse(
                await _authService.ExternalLoginAsync(request.Provider, request.IdToken)));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            return Ok(new GenericResponse(await _authService.LoginAsync(request.Email, request.Password)));
        }

        [HttpPut, Authorize]
        public IActionResult Revoke(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            _userService.RevokeRefreshToken(refreshToken);
            return Ok(new GenericResponse(SuccessResponseMessages.TokenRevoked));
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Validate(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            var user = await _userService.GetByIdAsync(id);
            return Ok(new GenericResponse(user));
        }


        [HttpPut]
        public async Task<IActionResult> Rotate(RotateTokenRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var response = await _userService.RotateTokensAsync(request);
            return Ok(new GenericResponse(response));
        }
    }
}