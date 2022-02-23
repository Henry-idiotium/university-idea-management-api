using System.Collections.Generic;
using System.Threading.Tasks;
using UIM.Model.Entities;

namespace UIM.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddRefreshTokenAsync(AppUser user, RefreshToken refreshToken);
        AppUser GetByRefreshToken(string token);
        RefreshToken GetRefreshToken(string token);
        Task<bool> RemoveOutdatedRefreshTokensAsync(AppUser user);
        Task<bool> RevokeRefreshTokenAsync(RefreshToken token, string reason = null, string replacedByToken = null);
        Task<bool> RevokeRefreshTokenDescendantsAsync(RefreshToken token, AppUser user, string reason);
        bool ValidateExistence(AppUser user);
    }
}