namespace UIM.Core.Models.Entities;

public class Like : NonEntity
{
    public string UserId { get; set; } = default!;
    public string IdeaId { get; set; } = default!;
    public bool IsLike { get; set; }

    public virtual AppUser User { get; set; } = default!;
    public virtual Idea Idea { get; set; } = default!;
}