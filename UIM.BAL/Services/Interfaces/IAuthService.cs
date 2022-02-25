using System.Threading.Tasks;
using UIM.Model.Dtos.Auth;

namespace UIM.BAL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> ExternalLoginAsync(string provider, string idToken);
        Task<AuthResponse> LoginAsync(string email, string password);
    }
}