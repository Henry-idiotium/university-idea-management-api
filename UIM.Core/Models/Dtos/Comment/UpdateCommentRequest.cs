using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Comment;

public class UpdateCommentRequest : CommentDto
{
    [TJS.JsonIgnore]
    public string Id { get; set; } = default!;
    [TJS.JsonIgnore]
    public string UserId { get; set; } = default!;
}
