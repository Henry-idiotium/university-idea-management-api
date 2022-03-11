namespace UIM.Core.Models.Entities;

public class Attachment : Entity<int>
{
    public string IdeaId { get; set; } = default!;
    public string Url { get; set; } = default!;

    public virtual Idea Idea { get; set; } = default!;
}