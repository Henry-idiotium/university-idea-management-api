namespace UIM.Model.Dtos.Auth
{
    public class AuthResponse
    {
        public AuthResponse(Info userInfo)
        {
            UserInfo = userInfo;
        }

        public Info UserInfo { get; set; }

        public class Info
        {
            public string Id { get; set; }
            public string Email { get; set; }
        }
    }
}