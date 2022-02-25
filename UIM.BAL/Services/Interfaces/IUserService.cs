using System.Threading.Tasks;
using Sieve.Models;
using UIM.Model.Dtos.Auth;
using UIM.Model.Dtos.Common;
using UIM.Model.Dtos.Token;
using UIM.Model.Dtos.User;

namespace UIM.BAL.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateAsync(CreateUserRequest request);
        Task DeleteAsync(string userId);
        Task<UserDetailsResponse> GetByIdAsync(string userId);
        Task<TableResponse> GetUsersAsync(SieveModel model);
        Task RevokeRefreshToken(string token);
        Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request);
        Task UpdateInfoAsync(string userId, UpdateUserInfoRequest request);
        Task UpdatePasswordAsync(string userId, UpdateUserPasswordRequest request);
    }
}