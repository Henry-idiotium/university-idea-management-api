namespace UIM.Core.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public AuthService(
        IJwtService jwtService,
        UserManager<AppUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> ExternalLoginAsync(ExternalAuthRequest request)
    {
        var payload = await _jwtService.VerifyGoogleToken(request.IdToken);

        var user = await _userManager.FindByEmailAsync(payload.Email);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
        await _unitOfWork.Users.AddRefreshTokenAsync(user, refreshToken);
        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);

        return new(accessToken, refreshToken.Token);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request?.Email);
        var pwdCorrect = await _userManager.CheckPasswordAsync(user, request?.Password);
        if (user == null || !pwdCorrect)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
        await _unitOfWork.Users.AddRefreshTokenAsync(user, refreshToken);

        return new(accessToken, refreshToken.Token);
    }

    public async Task UpdatePasswordAsync(string userId, UpdatePasswordRequest request)
    {
        if (request?.Password != request?.ConfirmPassword)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var user = await _userManager.FindByIdAsync(userId);
        var pwdCorrect = await _userManager.CheckPasswordAsync(user, request?.Password);
        if (!pwdCorrect)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var resetResult = await _userManager.ResetPasswordAsync(user,
            request?.PasswordResetToken,
            request?.Password);

        if (!resetResult.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }

    public async Task RevokeRefreshToken(string token)
    {
        var refreshToken = _unitOfWork.Users.GetRefreshToken(token);
        if (!refreshToken.IsActive)
            throw new HttpException(HttpStatusCode.Forbidden,
                                    ErrorResponseMessages.Forbidden);

        await _unitOfWork.Users.RevokeRefreshTokenAsync(refreshToken, "Revoked without replacement");
    }

    public async Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request)
    {
        var user = _unitOfWork.Users.GetByRefreshToken(request.RefreshToken);
        var ownedRefreshToken = _unitOfWork.Users.GetRefreshToken(request.RefreshToken);
        if (ownedRefreshToken == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        if (ownedRefreshToken.IsRevoked)
            // revoke all descendant tokens in case this token has been compromised
            await _unitOfWork.Users.RevokeRefreshTokenDescendantsAsync(ownedRefreshToken, user,
                reason: $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");

        if (!ownedRefreshToken.IsActive)
            throw new HttpException(HttpStatusCode.Forbidden,
                                    ErrorResponseMessages.Forbidden);

        // rotate token
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
        await _unitOfWork.Users.RevokeRefreshTokenAsync(
            token: ownedRefreshToken,
            reason: "Replaced by new token",
            replacedByToken: refreshToken.Token);
        await _unitOfWork.Users.RemoveOutdatedRefreshTokensAsync(user);

        // Get principal from expired token
        var principal = _jwtService.GetClaimsPrincipal(request.AccessToken);
        if (principal == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var accessToken = _jwtService.GenerateAccessToken(principal.Claims);
        return new(accessToken, refreshToken.Token);
    }
}