namespace UIM.Core.Models.Dtos.Token;

public class RotateTokenRequest
{
    public RotateTokenRequest(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
