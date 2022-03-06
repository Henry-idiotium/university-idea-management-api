using System.Threading.Tasks;
using UIM.Core.Models.Dtos.Auth;
using UIM.Core.Models.Dtos.Token;

namespace UIM.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> ExternalLoginAsync(string provider, string idToken);
        Task<AuthResponse> LoginAsync(string email, string password);
        Task RevokeRefreshToken(string token);
        Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request);
    }
}