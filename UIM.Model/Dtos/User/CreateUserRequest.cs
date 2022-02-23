using System;
using System.ComponentModel.DataAnnotations;

namespace UIM.Model.Dtos.User
{
    public class CreateUserRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string FullName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string ConfirmPassword { get; set; }
        [Required] public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
    }
}