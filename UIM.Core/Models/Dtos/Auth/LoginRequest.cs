namespace UIM.Core.Models.Dtos.Auth;

public class LoginRequest
{
    [Required] public string Email { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
}