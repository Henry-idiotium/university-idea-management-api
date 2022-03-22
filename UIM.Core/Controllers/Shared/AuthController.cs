namespace UIM.Core.Controllers.Shared;

public class AuthController : SharedController<IAuthService>
{
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;


    public AuthController(IAuthService authService, IUserService userService, IJwtService jwtService)
        : base(authService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpGet("[controller]/info")]
    public async Task<IActionResult> GetMeData()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var user = await _userService.FindByIdAsync(userId);
        return ResponseResult(user);
    }

    [HttpPost("[controller]/update-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].First().Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        await _service.UpdatePasswordAsync(userId, request);
        return ResponseResult();
    }

    [AllowAnonymous]
    [HttpPost("[controller]/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var response = await _service.LoginAsync(request);
        return ResponseResult(response);
    }

    [AllowAnonymous]
    [HttpPost("[controller]/login-ex")]
    public async Task<IActionResult> LoginExternal([FromBody] ExternalAuthRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var response = await _service.ExternalLoginAsync(request);
        return ResponseResult(response);
    }

    [HttpPut("[controller]/token/revoke/{token}")]
    public IActionResult Revoke(string token)
    {
        _service.RevokeRefreshToken(token);
        return ResponseResult(SuccessResponseMessages.TokenRevoked);
    }

    [HttpPut("[controller]/token/rotate")]
    public async Task<IActionResult> Rotate([FromBody] RotateTokenRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var response = await _service.RotateTokensAsync(request);
        return ResponseResult(response);
    }
}