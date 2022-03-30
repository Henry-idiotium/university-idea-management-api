using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Tag;

public class UpdateTagRequest : TagDto, IUpdateRequest
{
    [TJS.JsonIgnore] public string? Id { get; set; }
    [TJS.JsonIgnore] public string? UserId { get; set; }
}