namespace UIM.Core.Services.Interfaces;

public interface IJwtService
{
    AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
    Task<AccessToken> GenerateAccessTokenAsync(string userId);
    RefreshToken GenerateRefreshToken(string userId);
    ClaimsPrincipal? GetClaimsPrincipal(string? token);
    string? Validate(string? token);
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
}