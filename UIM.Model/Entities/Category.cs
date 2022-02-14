using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UIM.Model.Entities
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        // Referential Integrity
        public ICollection<Idea> Idea { get; set; }
    }
}