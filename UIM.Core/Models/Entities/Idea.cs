namespace UIM.Core.Models.Entities;

public class Idea : Entity
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsAnonymous { get; set; }
    public string? UserId { get; set; }
    public string SubmissionId { get; set; } = default!;

    public virtual AppUser? User { get; set; }
    public virtual ICollection<IdeaTag>? IdeaTags { get; set; }
    public virtual Submission Submission { get; set; } = default!;
    public virtual List<Comment> Comments { get; set; } = default!;
    public virtual List<Attachment> Attachments { get; set; } = default!;
    public virtual ICollection<Like>? Likes { get; set; }
    public virtual ICollection<View>? Views { get; set; }
}
