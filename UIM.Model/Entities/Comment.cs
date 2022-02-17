using System;
using System.ComponentModel.DataAnnotations;

namespace UIM.Model.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        public string IdeaId { get; set; }
        public string Parrent { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Referential Integrity
        public Idea Idea { get; set; }
    }
}