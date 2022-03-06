using System.ComponentModel.DataAnnotations;
using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.User
{
    public class CreateUserRequest : UserDto, ICreateRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string ConfirmPassword { get; set; }
        public string RoleId { get; set; }
        [Required] public int DepartmentId { get; set; }
    }
}