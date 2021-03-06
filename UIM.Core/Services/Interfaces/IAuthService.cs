namespace UIM.Core.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> ExternalLoginAsync(ExternalAuthRequest request);
    Task<UserDetailsResponse> GetMeDataAsync(string id);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task RevokeRefreshToken(string token);
    Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request);
    Task UpdatePasswordAsync(UpdatePasswordRequest request);
}
