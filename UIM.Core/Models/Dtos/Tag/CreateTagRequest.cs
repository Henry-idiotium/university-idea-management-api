using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Idea;

public class CreateTagRequest : TagDto, ICreateRequest
{
    [TJS.JsonIgnore] public string? UserId { get; set; }
}