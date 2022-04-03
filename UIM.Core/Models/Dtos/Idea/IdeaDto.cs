namespace UIM.Core.Models.Dtos.Idea;

public abstract class IdeaDto
{
    [Required]
    public virtual string Title { get; set; } = default!;
    [Required]
    public virtual string Content { get; set; } = default!;
    public virtual bool IsAnonymous { get; set; }
    public virtual string[] Tags { get; set; } = default!;
}
