using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UIM.Model.Entities
{
    public class Idea
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public string SubmissionId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Referential Integrities
        public AppUser User { get; set; }
        public Submission Submission { get; set; }
        public Category Category { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}