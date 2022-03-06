using UIM.Core.Common;
using UIM.Core.Models.Dtos;

namespace UIM.Core.Models.Entities
{
    public class Like : Entity
    {
        public string UserId { get; set; }
        public string IdeaId { get; set; }
        public bool IsLike { get; set; }

        // Referential Integrities
        public AppUser User { get; set; }
        public Idea Idea { get; set; }
    }
}