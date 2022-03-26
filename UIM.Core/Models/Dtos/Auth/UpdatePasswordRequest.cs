namespace UIM.Core.Models.Dtos.Auth;

public class UpdatePasswordRequest
{
    [Required] public string OldPassword { get; set; } = default!;
    [Required] public string NewPassword { get; set; } = default!;
    [Required] public string ConfirmNewPassword { get; set; } = default!;
}
