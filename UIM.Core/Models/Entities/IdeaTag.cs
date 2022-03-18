namespace UIM.Core.Models.Entities;

public class IdeaTag : BaseEntity
{
    public string TagId { get; set; } = default!;
    public string IdeaId { get; set; } = default!;

    public virtual Tag? Tag { get; set; }
    public virtual Idea? Idea { get; set; }
}