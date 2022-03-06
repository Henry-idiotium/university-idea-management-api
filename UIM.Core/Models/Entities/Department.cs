using System.Collections.Generic;
using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class Department : Entity<int>
    {
        public string Name { get; set; }

        // Referential Integrity
        public ICollection<AppUser> Users { get; set; }
    }
}