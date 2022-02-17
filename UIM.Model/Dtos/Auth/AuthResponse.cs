using UIM.Model.Dtos.Token;

namespace UIM.Model.Dtos.Auth
{
    public class AuthResponse
    {
        public AuthResponse(UserDetailsResponse userInfo)
        {
            UserInfo = userInfo;
        }

        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDetailsResponse UserInfo { get; set; }
    }
}