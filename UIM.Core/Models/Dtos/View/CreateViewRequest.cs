using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.View;

public class CreateViewRequest
{
    [TJS.JsonIgnore]
    public string? IdeaId { get; set; }
    [TJS.JsonIgnore]
    public string? UserId { get; set; }
}
