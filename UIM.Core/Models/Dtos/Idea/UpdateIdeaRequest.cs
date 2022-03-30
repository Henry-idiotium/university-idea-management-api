using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Idea;

public class UpdateIdeaRequest : IdeaDto, IUpdateRequest
{
    [TJS.JsonIgnore] public string? Id { get; set; }
    [TJS.JsonIgnore] public string? UserId { get; set; }
}