using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Auth;

public class UpdatePasswordRequest
{
    [TJS.JsonIgnore]
    [Required]
    public string Id { get; set; } = default!;
    [Required]
    public string OldPassword { get; set; } = default!;
    [Required]
    public string NewPassword { get; set; } = default!;
    [Required]
    public string ConfirmNewPassword { get; set; } = default!;
}
