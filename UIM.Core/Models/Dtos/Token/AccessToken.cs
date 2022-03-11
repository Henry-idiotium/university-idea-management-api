namespace UIM.Core.Models.Dtos.Token;

public class AccessToken
{
    public AccessToken(string type, string token, string expiredAt)
    {
        Token = token;
        Type = type;
        ExpiredAt = expiredAt;
    }

    public string Type { get; set; }
    public string Token { get; set; }
    public string ExpiredAt { get; set; }
}