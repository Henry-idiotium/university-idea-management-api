using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Comment;

public class CreateCommentRequest : CommentDto
{
    [Required]
    public string? IdeaId { get; set; }
    [TJS.JsonIgnore]
    public string? UserId { get; set; }
}
