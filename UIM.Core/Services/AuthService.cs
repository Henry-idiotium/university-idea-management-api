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
            throw new HttpException(HttpStatusCode.Unauthorized);

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
            throw new HttpException(HttpStatusCode.Unauthorized);

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
        await _unitOfWork.Users.AddRefreshTokenAsync(user, refreshToken);

        return new(accessToken, refreshToken.Token);
    }

    public async Task UpdatePasswordAsync(UpdatePasswordRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        if (request.NewPassword != request.ConfirmNewPassword)
            throw new HttpException(HttpStatusCode.BadRequest);

        var user = await _userManager.FindByIdAsync(request.Id);
        var pwdCorrect = await _userManager.CheckPasswordAsync(user, request?.OldPassword);
        if (!pwdCorrect)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var pwdReset = await _userManager.ResetPasswordAsync(user, token, request?.NewPassword);

        if (!pwdReset.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);

        user.IsDefaultPassword = false;
        var userUpdated = await _userManager.UpdateAsync(user);
        if (!userUpdated.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task RevokeRefreshToken(string token)
    {
        var refreshToken = _unitOfWork.Users.GetRefreshToken(token);
        if (!refreshToken.IsActive)
            throw new HttpException(HttpStatusCode.Forbidden);

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
            throw new HttpException(HttpStatusCode.Forbidden);

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