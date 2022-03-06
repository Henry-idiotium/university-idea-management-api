using System;
using System.ComponentModel.DataAnnotations;

namespace UIM.Core.Models.Dtos.User
{
    public abstract class UserDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}