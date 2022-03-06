using System;
using System.Collections.Generic;
using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class Submission : Entity<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public bool IsActive { get; set; }

        // Referential Integrity
        public ICollection<Idea> Ideas { get; set; }
    }
}