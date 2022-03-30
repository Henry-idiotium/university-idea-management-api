using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.User;

public class UpdateUserRequest : UserDto, IUpdateRequest
{
    [TJS.JsonIgnore] public string? Id { get; set; }
}