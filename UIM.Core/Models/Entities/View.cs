namespace UIM.Core.Models.Entities;

public class View : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string IdeaId { get; set; } = default!;

    public virtual AppUser? User { get; set; }
    public virtual Idea? Idea { get; set; }
}
