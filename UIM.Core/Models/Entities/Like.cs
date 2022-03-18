namespace UIM.Core.Models.Entities;

public class Like : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string IdeaId { get; set; } = default!;
    public bool IsLike { get; set; }

    public virtual AppUser? User { get; set; }
    public virtual Idea? Idea { get; set; }
}