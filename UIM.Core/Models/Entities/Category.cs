using System.Collections.Generic;
using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class Category : Entity<int>
    {
        public string Name { get; set; }

        // Referential Integrity
        public ICollection<Idea> Ideas { get; set; }
    }
}