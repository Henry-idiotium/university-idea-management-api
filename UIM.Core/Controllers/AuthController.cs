using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UIM.Core.Helpers;
using UIM.Core.Helpers.Attributes;
using UIM.Core.Models.Dtos;
using UIM.Core.Models.Dtos.Auth;
using UIM.Core.Models.Dtos.Token;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Controllers
{
    [JwtAuthorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService, IJwtService jwtService)
        {
            _authService = authService;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetMeData()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var userId = _jwtService.Validate(token);
            if (userId == null)
                throw new HttpException(HttpStatusCode.Unauthorized,
                                        ErrorResponseMessages.Unauthorized);

            var user = await _userService.FindByIdAsync(userId);
            return Ok(new GenericResponse(user));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var response = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new GenericResponse(response));
        }

        [AllowAnonymous]
        [HttpPost("login-ex")]
        public async Task<IActionResult> LoginExternal([FromBody] ExternalAuthRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            return Ok(new GenericResponse(
                await _authService.ExternalLoginAsync(request.Provider, request.IdToken)));
        }

        [HttpPut("token/revoke")]
        public IActionResult Revoke(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            _authService.RevokeRefreshToken(refreshToken);
            return Ok(new GenericResponse(SuccessResponseMessages.TokenRevoked));
        }

        [HttpPut("token/rotate")]
        public async Task<IActionResult> Rotate([FromBody] RotateTokenRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var response = await _authService.RotateTokensAsync(request);
            return Ok(new GenericResponse(response));
        }
    }
}