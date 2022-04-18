namespace UIM.Core.Models.Entities;

public class Comment : Entity
{
    public string Content { get; set; } = default!;
    public string IdeaId { get; set; } = default!;
    public string? UserId { get; set; }

    public virtual Idea Idea { get; set; } = default!;
    public virtual AppUser? User { get; set; }
}
