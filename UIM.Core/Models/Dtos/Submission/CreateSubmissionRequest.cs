using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Submission;

public class CreateSubmissionRequest : SubmissionDto, ICreateRequest
{
    [TJS.JsonIgnore] public string UserId { get; set; } = default!;
}