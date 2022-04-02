namespace UIM.Core.Models.Dtos.Comment;

public class CommentDetailsResponse : CommentDto
{
    public string? Id { get; set; }
    public string? IdeaId { get; set; }
    public string? UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
