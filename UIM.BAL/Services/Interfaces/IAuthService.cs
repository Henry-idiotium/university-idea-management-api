using System.Threading.Tasks;
using UIM.Model.Dtos.Auth;

namespace UIM.BAL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}