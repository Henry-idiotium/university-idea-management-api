using System.Collections.Generic;
using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class Idea : Entity<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public string UserId { get; set; }
        public int? CategoryId { get; set; }
        public string SubmissionId { get; set; }

        // Referential Integrities
        public AppUser User { get; set; }
        public Submission Submission { get; set; }
        public Category Category { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<View> Views { get; set; }
        public List<Comment> Comments { get; set; }
    }
}