using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class Comment : Entity<string>
    {
        public string IdeaId { get; set; }
        public string Parrent { get; set; }
        public string Content { get; set; }

        // Referential Integrity
        public Idea Idea { get; set; }
    }
}