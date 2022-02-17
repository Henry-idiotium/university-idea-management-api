using System.Collections.Generic;

namespace UIM.Model.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Referential Integrity
        public ICollection<AppUser> Users { get; set; }
    }
}