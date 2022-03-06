using System.ComponentModel.DataAnnotations;

namespace UIM.Core.Models.Dtos.Idea
{
    public abstract class IdeaDto
    {
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public int? CategoryId { get; set; }
        public string SubmissionId { get; set; }
    }
}