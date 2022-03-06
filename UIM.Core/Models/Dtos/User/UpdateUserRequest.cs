using System.ComponentModel.DataAnnotations;
using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.User
{
    public class UpdateUserRequest : UserDto, IUpdateRequest
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string RoleId { get; set; }
        [Required] public int DepartmentId { get; set; }
    }
}