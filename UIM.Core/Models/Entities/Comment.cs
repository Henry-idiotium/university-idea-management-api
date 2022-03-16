namespace UIM.Core.Models.Entities;

public class Comment : Entity
{
    public string IdeaId { get; set; } = default!;
    public string? Parrent { get; set; }
    public string Content { get; set; } = default!;

    public virtual Idea Idea { get; set; } = default!;
}