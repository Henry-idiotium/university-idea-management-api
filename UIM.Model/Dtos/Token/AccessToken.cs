namespace UIM.Model.Dtos.Token
{
    public class AccessToken
    {
        public AccessToken(string token, string expiredAt)
        {
            Token = token;
            ExpiredAt = expiredAt;
        }

        public string Token { get; set; }
        public string ExpiredAt { get; set; }
    }
}