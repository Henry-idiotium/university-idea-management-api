namespace UIM.Core.Controllers;

[JwtAuthorize(RoleNames.Admin)]
[Route("api/auth")]
public class AuthController : UimController
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
        return ResponseResult(user);
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized,
                                    ErrorResponseMessages.Unauthorized);

        await _authService.UpdatePasswordAsync(userId, request);
        return ResponseResult();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var response = await _authService.LoginAsync(request);
        return ResponseResult(response);
    }

    [AllowAnonymous]
    [HttpPost("login-ex")]
    public async Task<IActionResult> LoginExternal([FromBody] ExternalAuthRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var response = await _authService.ExternalLoginAsync(request);
        return ResponseResult(response);
    }

    [HttpPut("token/revoke")]
    public IActionResult Revoke(string refreshToken)
    {
        _authService.RevokeRefreshToken(refreshToken);
        return ResponseResult(SuccessResponseMessages.TokenRevoked);
    }

    [HttpPut("token/rotate")]
    public async Task<IActionResult> Rotate([FromBody] RotateTokenRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var response = await _authService.RotateTokensAsync(request);
        return ResponseResult(response);
    }
}