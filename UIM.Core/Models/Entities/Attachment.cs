using System;
using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class Attachment : Entity<int>
    {
        public string IdeaId { get; set; }
        public string Url { get; set; }

        // Referential Integrity
        public Idea Idea { get; set; }
    }
}