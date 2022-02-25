using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UIM.Model.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public string IdeaId { get; set; }
        public string Url { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Referential Integrity
        public Idea Idea { get; set; }
    }
}