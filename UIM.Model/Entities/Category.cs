using System.Collections.Generic;

namespace UIM.Model.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Referential Integrity
        public ICollection<Idea> Ideas { get; set; }
    }
}