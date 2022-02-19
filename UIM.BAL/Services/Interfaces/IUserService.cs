using System.Threading.Tasks;
using UIM.Model.Dtos.Auth;
using UIM.Model.Dtos.Token;
using UIM.Model.Entities;

namespace UIM.BAL.Services.Interfaces
{
    public interface IUserService
    {
        Task AddRefreshTokenAsync(AppUser user, RefreshToken refreshToken);
        Task RevokeRefreshToken(string token);
        Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request);
    }
}