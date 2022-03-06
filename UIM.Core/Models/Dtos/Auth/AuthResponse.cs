using UIM.Core.Models.Dtos.Token;
using UIM.Core.Models.Dtos.User;

namespace UIM.Core.Models.Dtos.Auth
{
    public class AuthResponse
    {
        public AuthResponse(AccessToken accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}