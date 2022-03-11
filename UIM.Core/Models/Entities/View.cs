namespace UIM.Core.Models.Entities;

public class View : Entity
{
    public string UserId { get; set; } = default!;
    public string IdeaId { get; set; } = default!;

    // Referential Integrities
    public virtual AppUser User { get; set; } = default!;
    public virtual Idea Idea { get; set; } = default!;
}