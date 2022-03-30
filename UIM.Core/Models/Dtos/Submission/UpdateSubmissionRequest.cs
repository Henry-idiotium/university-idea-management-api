using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Submission;

public class UpdateSubmissionRequest : SubmissionDto, IUpdateRequest
{
    [TJS.JsonIgnore] public string? Id { get; set; }
    [TJS.JsonIgnore] public string? UserId { get; set; }
}