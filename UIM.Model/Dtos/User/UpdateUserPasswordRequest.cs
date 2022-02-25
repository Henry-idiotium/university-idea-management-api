using System.ComponentModel.DataAnnotations;

namespace UIM.Model.Dtos.User
{
    public class UpdateUserPasswordRequest
    {
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required] public string ConfirmNewPassword { get; set; }
    }
}