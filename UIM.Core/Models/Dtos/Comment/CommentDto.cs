namespace UIM.Core.Models.Dtos.Idea;

public abstract class CommentDto
{
    [Required]
    public virtual string Content { get; set; } = default!;
}
