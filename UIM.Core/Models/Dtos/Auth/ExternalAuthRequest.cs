namespace UIM.Core.Models.Dtos.Auth
{
    public class ExternalAuthRequest
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }
    }
}