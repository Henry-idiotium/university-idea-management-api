using UIM.Model.Dtos.Token;

namespace UIM.Model.Dtos.Auth
{
    public class AuthResponse
    {
        public AuthResponse(UserDetailsResponse userInfo, AccessToken accessToken, string refreshToken)
        {
            UserInfo = userInfo;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDetailsResponse UserInfo { get; set; }
    }
}