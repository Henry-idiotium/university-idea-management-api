namespace UIM.Core.Models.Dtos.Auth;

public class ExternalAuthRequest
{
    [Required]
    public string Provider { get; set; } = default!;
    [Required]
    public string IdToken { get; set; } = default!;
}
