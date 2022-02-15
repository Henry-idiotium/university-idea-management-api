using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UIM.Model.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }

        // Referential Integrity
        public Department Department { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Idea> Ideas { get; set; }
    }
}