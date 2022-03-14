namespace UIM.Core.Models.Dtos.Auth;

public class UpdatePasswordRequest
{
    [Required] public string PasswordResetToken { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
    [Required] public string ConfirmPassword { get; set; } = default!;
}
