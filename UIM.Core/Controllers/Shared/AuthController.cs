namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class AuthController : SharedController<IAuthService>
{
    private readonly IJwtService _jwtService;

    public AuthController(IAuthService authService, IJwtService jwtService) : base(authService)
    {
        _jwtService = jwtService;
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetMeData()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var user = await _service.GetMeDataAsync(userId);
        return ResponseResult(user);
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].First().Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.Id = userId;

        await _service.UpdatePasswordAsync(request);
        return ResponseResult();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        request.Password = EncryptHelpers.DecryptRsa(request.Password);

        var response = await _service.LoginAsync(request);
        return ResponseResult(response);
    }

    [AllowAnonymous]
    [HttpPost("login-ex")]
    public async Task<IActionResult> LoginExternal([FromBody] ExternalAuthRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var response = await _service.ExternalLoginAsync(request);
        return ResponseResult(response);
    }

    [HttpPut("token/revoke/{token}")]
    public IActionResult Revoke(string token)
    {
        _service.RevokeRefreshToken(token);
        return ResponseResult(SuccessResponseMessages.TokenRevoked);
    }

    [AllowAnonymous]
    [HttpPut("token/rotate")]
    public async Task<IActionResult> Rotate([FromBody] RotateTokenRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var response = await _service.RotateTokensAsync(request);
        return ResponseResult(response);
    }
}
