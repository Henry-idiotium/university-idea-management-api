using System;

namespace UIM.Model.Entities
{
    public class View
    {
        public string UserId { get; set; }
        public string IdeaId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Referential Integrities
        public AppUser User { get; set; }
        public Idea Idea { get; set; }
    }
}