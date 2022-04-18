namespace UIM.Core.Models.Dtos.Comment;

public class CommentDetailsResponse : CommentDto
{
    public string? Id { get; set; }
    public SimpleUserResponse? User { get; set; }
    public SimpleIdeaResponse? Idea { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
