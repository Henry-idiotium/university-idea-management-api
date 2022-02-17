using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public ICollection<Idea> Idea { get; set; }
    }
}