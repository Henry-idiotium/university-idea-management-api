using System;
using System.Collections.Generic;

namespace UIM.Model.Entities
{
    public class Submission
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Referential Integrity
        public ICollection<Idea> Ideas { get; set; }
    }
}