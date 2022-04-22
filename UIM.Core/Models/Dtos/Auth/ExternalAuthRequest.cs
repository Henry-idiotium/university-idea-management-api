namespace UIM.Core.Models.Dtos.Auth;

public class ExternalAuthRequest
{
    [Required]
    public string IdToken { get; set; } = default!;
}
