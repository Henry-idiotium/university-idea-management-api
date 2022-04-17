using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Like;

public class CreateLikeRequest
{
    [TJS.JsonIgnore]
    public string? UserId { get; set; }
    [TJS.JsonIgnore]
    public string? IdeaId { get; set; }
    public bool? IsLike { get; set; }
}
