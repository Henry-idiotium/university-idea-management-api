namespace UIM.Core.Models.Entities;

public class Tag : Entity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<IdeaTag>? IdeaTags { get; set; }
}
