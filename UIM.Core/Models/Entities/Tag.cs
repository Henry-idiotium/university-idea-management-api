namespace UIM.Core.Models.Entities;

public class Tag : Entity
{
    public Tag(string name) => Name = name;

    public string Name { get; set; } = default!;

    public virtual ICollection<IdeaTag>? IdeaTags { get; set; }
}