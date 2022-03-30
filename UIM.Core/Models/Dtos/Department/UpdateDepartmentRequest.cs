using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Department;

public class UpdateDepartmentRequest : DepartmentDto, IUpdateRequest
{
    [TJS.JsonIgnore] public string? Id { get; set; }
    [TJS.JsonIgnore] public string? UserId { get; set; }
}