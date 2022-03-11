namespace UIM.Core.Models.Dtos.Idea;

public abstract class IdeaDto
{
    [Required] public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Content { get; set; } = default!;
    public bool IsAnonymous { get; set; }
    public int? CategoryId { get; set; }
    [Required] public string SubmissionId { get; set; } = default!;
}