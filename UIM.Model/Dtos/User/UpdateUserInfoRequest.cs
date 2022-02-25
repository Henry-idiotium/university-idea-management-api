using System;
using System.ComponentModel.DataAnnotations;

namespace UIM.Model.Dtos.User
{
    public class UpdateUserInfoRequest
    {
        [Required] public string FullName { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int NewDepartmentId { get; set; }
        public string NewRoleId { get; set; }
    }
}