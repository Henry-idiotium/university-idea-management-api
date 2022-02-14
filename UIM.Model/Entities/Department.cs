using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UIM.Model.Entities
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        // Referential Integrity
        public ICollection<AppUser> AppUser { get; set; }
    }
}