namespace UIM.Core.Models.Dtos.Idea;

public abstract class IdeaDto
{
    [Required] public virtual string Title { get; set; } = default!;
    [Required] public virtual string Content { get; set; } = default!;
    [Required] public virtual string SubmissionId { get; set; } = default!;
    public virtual string Description { get; set; } = default!;
    public virtual bool IsAnonymous { get; set; }
}